using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using OpenMcdf;

namespace RevitLauncher.Core
{
    public class RevitFileInfo
    {
        private const string _infoStream = "BasicFileInfo";
        public string Username { get; private set; } = string.Empty;
        public string CentralPath { get; private set; } = string.Empty;
        public string LanguageWhenSaved { get; private set; } = string.Empty;
        public Guid LatestCentralEpisodeGUID { get; private set; } = Guid.Empty;
        public int LatestCentralVersion { get; private set; } = 0;
        public string SavedInVersion { get; private set; } = string.Empty;
        public bool AllLocalChangesSavedToCentral { get; private set; } = false;
        public bool IsCentral { get; private set; } = false;
        public bool IsLocal { get; private set; } = false;
        public bool IsWorkshared { get; private set; } = false;

        public RevitFileInfo(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Init(fileStream);
        }

        public RevitFileInfo(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CompoundFile compoundFile = new CompoundFile(stream, CFSUpdateMode.ReadOnly, CFSConfiguration.Default);
            CFStream cfStream = compoundFile.RootStorage.GetStream(_infoStream);
            string rawString = Encoding.UTF8.GetString(cfStream.GetData());
            compoundFile.Close();

            var fileInfoDatas = rawString.Replace("\0", "").Split(new string[] { "\r\n", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var info in fileInfoDatas)
            {
                if (info.Contains("Worksharing:"))
                {
                    string workshare = Behind(info, "Worksharing:");
                    if (workshare.Contains("Local"))
                    {
                        IsLocal = true;
                    }
                    else if (workshare.Contains("Central"))
                    {
                        IsCentral = true;
                    }
                }
                else if (info.Contains("Username:"))
                {
                    Username = Behind(info, "Username:");
                }
                else if (info.Contains("Central File Path:"))
                {
                    CentralPath = Behind(info, "Central File Path:");
                }
                else if (info.Contains("Central Model Path:"))
                {
                    CentralPath = Behind(info, "Central Model Path:");
                }
                else if (info.Contains("Revit Build: "))
                {
                    string version = Behind(info, "Revit Build: ");
                    if (version.Contains("Autodesk Revit "))
                    {
                        SavedInVersion = Behind(version, "Autodesk Revit ").Substring(0, 4);
                    }
                    else
                    {
                        SavedInVersion = "2009";
                        break;
                    }
                }
                else if (info.Contains("Format: "))
                {
                    string version = Behind(info, "Format: ");
                    SavedInVersion = version;
                    break;
                }
                else if (info.Contains("Locale when saved: "))
                {
                    LanguageWhenSaved = Behind(info, "Locale when saved: ");
                }
                else if (info.Contains("All Local Changes Saved To Central:"))
                {
                    AllLocalChangesSavedToCentral = int.Parse(Behind(info, "All Local Changes Saved To Central:")) == 1;
                }
                else if (info.Contains("Central model's version number corresponding to the last reload latest:"))
                {
                    LatestCentralVersion = int.Parse(Behind(info, "Central model's version number corresponding to the last reload latest:"));
                }
                else if (info.Contains("Central model's episode GUID corresponding to the last reload latest:"))
                {
                    LatestCentralEpisodeGUID = Guid.Parse(Behind(info, "Central model's episode GUID corresponding to the last reload latest:"));
                }

            }
        }

        private static string Behind(string source, string key)
        {
            return source.Substring(source.LastIndexOf(key)).Remove(0, key.Length).Trim();
        }
    }
}
