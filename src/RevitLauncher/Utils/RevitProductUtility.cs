using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RevitLauncher.Utils
{
    /// <summary> Provides access to installed versions of Revit on the local machine. </summary>
    public sealed class RevitProductUtility
    {
        /// <summary> Gets a list of Revit products installed on this machine. </summary>
        /// <remarks>This operation parses the registry for necessary information about the installation(s).
        /// An incomplete or incorrect installation of Revit may not be returned correctly by this utility.</remarks>
        public static List<RevitProduct> GetAllInstalledRevitProducts()
        {
            List<RevitProduct> list = new List<RevitProduct>();
            RevitProductUtility.InitializeProductsDictionary();
            try
            {
                List<string> installedRevitProductCodes = RevitProductUtility.GetInstalledRevitProductCodes();
                foreach (string productCode in installedRevitProductCodes)
                {
                    RevitProduct revitProduct = RevitProductUtility.GetRevitProduct(productCode);
                    if (revitProduct != null)
                    {
                        list.Add(revitProduct);
                    }
                }
            }
            catch (Exception)
            {
            }

            return list;
        }

        internal static LanguageType ConvertLanguageCodeToType(int code)
        {
            switch (code)
            {
                case 1028:
                    return LanguageType.Chinese_Traditional;
                case 1029:
                    return LanguageType.Czech;
                case 1030:
                case 1032:
                case 1035:
                case 1037:
                case 1039:
                case 1043:
                case 1044:
                case 1047:
                case 1048:
                    break;
                case 1031:
                    return LanguageType.German;
                case 1033:
                    return LanguageType.English_USA;
                case 1034:
                    return LanguageType.Spanish;
                case 1036:
                    return LanguageType.French;
                case 1038:
                    return LanguageType.Hungarian;
                case 1040:
                    return LanguageType.Italian;
                case 1041:
                    return LanguageType.Japanese;
                case 1042:
                    return LanguageType.Korean;
                case 1045:
                    return LanguageType.Polish;
                case 1046:
                    return LanguageType.Brazilian_Portuguese;
                case 1049:
                    return LanguageType.Russian;
                default:
                    if (code == 2052)
                    {
                        return LanguageType.Chinese_Simplified;
                    }

                    if (code == 2057)
                    {
                        return LanguageType.English_GB;
                    }

                    break;
            }

            return LanguageType.Unknown;
        }

        internal static string ConvertLanguageTypeToCode(LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.English_USA:
                    return "0409";
                case LanguageType.German:
                    return "0407";
                case LanguageType.Spanish:
                    return "040A";
                case LanguageType.French:
                    return "040C";
                case LanguageType.Italian:
                    return "0410";
                case LanguageType.Chinese_Simplified:
                    return "0804";
                case LanguageType.Chinese_Traditional:
                    return "0404";
                case LanguageType.Japanese:
                    return "0411";
                case LanguageType.Korean:
                    return "0412";
                case LanguageType.Russian:
                    return "0419";
                case LanguageType.Czech:
                    return "0405";
                case LanguageType.Polish:
                    return "0415";
                case LanguageType.Hungarian:
                    return "040E";
                case LanguageType.Brazilian_Portuguese:
                    return "0416";
                case LanguageType.English_GB:
                    return "0809";
            }

            return "0409";
        }

        internal static RevitVersion ConverVersionCodeToVersion(int code)
        {
            switch (code)
            {
                case 11:
                    return RevitVersion.Revit2011;
                case 12:
                    return RevitVersion.Revit2012;
                case 13:
                    return RevitVersion.Revit2013;
                case 14:
                    return RevitVersion.Revit2014;
                case 15:
                    return RevitVersion.Revit2015;
                case 16:
                    return RevitVersion.Revit2016;
                case 17:
                    return RevitVersion.Revit2017;
                case 18:
                    return RevitVersion.Revit2018;
                case 19:
                    return RevitVersion.Revit2019;
                case 20:
                    return RevitVersion.Revit2020;
                case 21:
                    return RevitVersion.Revit2021;
                case 22:
                    return RevitVersion.Revit2022;
                case 23:
                    return RevitVersion.Revit2023;
                default:
                    return RevitVersion.Unknown;
            }
        }

        internal static ProductType ConvertDisciplineCodeToProductType(int code)
        {
            switch (code)
            {
                case 1:
                    return ProductType.Architecture;
                case 2:
                    return ProductType.Structure;
                case 3:
                    return ProductType.MEP;
                case 5:
                    return ProductType.Revit;
            }

            return ProductType.Unknown;
        }

        internal static string ConvertProductTypetoCode(ProductType product)
        {
            switch (product)
            {
                case ProductType.Architecture:
                    return "01";
                case ProductType.Structure:
                    return "02";
                case ProductType.MEP:
                    return "03";
                case ProductType.Revit:
                    return "05";
                default:
                    return "01";
            }
        }

        /// <summary>
        /// Initialize a dictionary to store all the RevitProducts which
        /// can be supported by this RevitAddInUtility
        ///
        /// NOTE: This method only initializes the product codes for Revit 2011 and RVT 2013.
        /// Since 2012 we analysis the Revit product code (not include the dynamo) of RevitDB.dll component with a fixed pattern,
        /// please see RevitProductUtility::getRevitProduct method for more details.
        /// </summary>
        private static void InitializeProductsDictionary()
        {
            if (RevitProductUtility._mProductsHashtable == null)
            {
                RevitProductUtility._mProductsHashtable = new Hashtable();
            }
            else
            {
                RevitProductUtility._mProductsHashtable.Clear();
            }

            RevitProductUtility._mProductsHashtable.Add("{4AF99FCA-1D0C-4D5A-9BFE-0D4376A52B23}",
                new RevitProduct(new Guid("{4AF99FCA-1D0C-4D5A-9BFE-0D4376A52B23}"), ProductType.Architecture,
                    AddInArchitecture.OS32bit, RevitVersion.Revit2011));
            RevitProductUtility._mProductsHashtable.Add("{0EE1FCA9-7474-4143-8F22-E7AD998FACBF}",
                new RevitProduct(new Guid("{0EE1FCA9-7474-4143-8F22-E7AD998FACBF}"), ProductType.Structure,
                    AddInArchitecture.OS32bit, RevitVersion.Revit2011));
            RevitProductUtility._mProductsHashtable.Add("{CCCB80C8-5CC5-4EB7-89D0-F18E405F18F9}",
                new RevitProduct(new Guid("{CCCB80C8-5CC5-4EB7-89D0-F18E405F18F9}"), ProductType.MEP,
                    AddInArchitecture.OS32bit, RevitVersion.Revit2011));
            RevitProductUtility._mProductsHashtable.Add("{7A177659-6ADE-439F-9B68-AAB03739A5CF}",
                new RevitProduct(new Guid("{7A177659-6ADE-439F-9B68-AAB03739A5CF}"), ProductType.Revit,
                    AddInArchitecture.OS32bit, RevitVersion.Revit2013));
            RevitProductUtility._mProductsHashtable.Add("{94D463D0-2B13-4181-9512-B27004B1151A}",
                new RevitProduct(new Guid("{94D463D0-2B13-4181-9512-B27004B1151A}"), ProductType.Architecture,
                    AddInArchitecture.OS64bit, RevitVersion.Revit2011));
            RevitProductUtility._mProductsHashtable.Add("{23853368-22DD-4817-904B-DB04ADE9B0C8}",
                new RevitProduct(new Guid("{23853368-22DD-4817-904B-DB04ADE9B0C8}"), ProductType.Structure,
                    AddInArchitecture.OS64bit, RevitVersion.Revit2011));
            RevitProductUtility._mProductsHashtable.Add("{C31F3560-0007-4955-9F65-75CB47F82DB5}",
                new RevitProduct(new Guid("{C31F3560-0007-4955-9F65-75CB47F82DB5}"), ProductType.MEP,
                    AddInArchitecture.OS64bit, RevitVersion.Revit2011));
            RevitProductUtility._mProductsHashtable.Add("{2F816EFF-FACE-4000-AC88-C9AAA4A05E5D}",
                new RevitProduct(new Guid("{2F816EFF-FACE-4000-AC88-C9AAA4A05E5D}"), ProductType.Revit,
                    AddInArchitecture.OS64bit, RevitVersion.Revit2013));
        }

        /// <summary> Gets information of installed Revit from registry. </summary>
        private static RevitProduct GetInstalledProductInfo(string productRegGuid)
        {
            if (RevitProductUtility._mProductsHashtable.ContainsKey(productRegGuid))
            {
                RevitProduct revitProduct = (RevitProduct)RevitProductUtility._mProductsHashtable[productRegGuid];
                RegistryKey registryKey = Registry.LocalMachine
                    .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall").OpenSubKey(productRegGuid);
                if (registryKey == null)
                {
                    string subKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + productRegGuid;
                    UIntPtr hKey;
                    if (Win32API.RegOpenKeyEx(RevitProductUtility._hkeyLocalMachine, subKey, 0, 131609,
                        out hKey) != 0 && Win32API.RegOpenKeyEx(RevitProductUtility._hkeyLocalMachine,
                        subKey, 0, 131353, out hKey) != 0)
                    {
                        return null;
                    }

                    uint num = 1024u;
                    uint code = 0u;
                    StringBuilder stringBuilder = new StringBuilder(1024);
                    uint num2;
                    Win32API.RegQueryValueEx(hKey, "InstallLocation", 0, out num2, stringBuilder, out num);
                    revitProduct.SetInstallLocation(stringBuilder.ToString());
                    num = 1024u;
                    Win32API.RegQueryValueEx(hKey, "DisplayName", 0, out num2, stringBuilder, out num);
                    revitProduct.SetName(stringBuilder.ToString());
                    Win32API.RegQueryValueEx(hKey, "Language", 0, out num2, out code, out num);
                    revitProduct.setLanguage(RevitProductUtility.ConvertLanguageCodeToType((int)code));
                    revitProduct.SetAllUsersAddInFolder(
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                        "\\Autodesk\\Revit\\AddIns\\" + revitProduct.Version.ToString().Replace("Revit", ""));
                    revitProduct.SetCurrentUserAddInFolder(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                        "\\Autodesk\\Revit\\AddIns\\" + revitProduct.Version.ToString().Replace("Revit", ""));
                    Win32API.RegCloseKey(hKey);
                }
                else
                {
                    revitProduct.SetInstallLocation((string)registryKey.GetValue("InstallLocation"));
                    revitProduct.SetName((string)registryKey.GetValue("DisplayName"));
                    revitProduct.setLanguage(
                        RevitProductUtility.ConvertLanguageCodeToType((int)registryKey.GetValue("Language")));
                    revitProduct.SetAllUsersAddInFolder(
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                        "\\Autodesk\\Revit\\AddIns\\" + revitProduct.Version.ToString().Replace("Revit", ""));
                    revitProduct.SetCurrentUserAddInFolder(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                        "\\Autodesk\\Revit\\AddIns\\" + revitProduct.Version.ToString().Replace("Revit", ""));
                    registryKey.Close();
                }

                return revitProduct;
            }

            return null;
        }

        /// <summary> Gets product codes of the installed Revits. </summary>
        public static List<string> GetInstalledRevitProductCodes()
        {
            List<string> list = new List<string>();
            string text = new string('0', GuidStringSize);
            uint num = 0u;
            while (Win32API.MsiEnumClients(RevitDBComponentID, num++, text) == 0u)
            {
                string item = string.Copy(text);
                list.Add(item);
            }

            return list;
        }

        /// <summary> Gets product information with the product code. </summary>
        private static RevitProduct GetRevitProduct(string productCode)
        {
            Regex regex =
                new Regex(
                    "^(\\{{0,1}(7346B4A[0-9a-fA-F])-(?<Majorversion>([0-9a-fA-F]){2})(?<Subversion>([0-9a-fA-F]){2})-(?<Discipline>([0-9a-fA-F]){2})(?<Platform>([0-9a-fA-F]){1})[0-9a-fA-F]-(?<Language>([0-9a-fA-F]){4})-705C0D862004\\}{0,1})$");
            Match match = regex.Match(productCode);
            if (match.Success)
            {
                int code = int.Parse(match.Result("${Majorversion}"));
                AddInArchitecture architecture = (match.Result("${Platform}").CompareTo("0") == 0)
                    ? AddInArchitecture.OS32bit
                    : AddInArchitecture.OS64bit;
                RevitVersion version = RevitProductUtility.ConverVersionCodeToVersion(code);
                int code2 = int.Parse(match.Result("${Discipline}"), NumberStyles.AllowHexSpecifier);
                ProductType product = RevitProductUtility.ConvertDisciplineCodeToProductType(code2);
                RevitProduct revitProduct = new RevitProduct(new Guid(productCode), product, architecture, version);
                int code3 = int.Parse(match.Result("${Language}"), NumberStyles.AllowHexSpecifier);
                revitProduct.setLanguage(RevitProductUtility.ConvertLanguageCodeToType(code3));
                revitProduct.setReleaseSubVersion("2021.0");
                string text = new string(' ', 260);
                string text2 = new string(' ', 260);
                int length = 260;
                int length2 = 260;
                Win32API.MsiGetProductInfo(productCode, "ProductName", text, out length);
                Win32API.MsiGetProductInfo(productCode, "InstallLocation", text2, out length2);
                text = text.Substring(0, length);
                text2 = text2.Substring(0, length2);
                revitProduct.SetName(text);
                revitProduct.SetInstallLocation(text2);
                uint subversion = (uint)int.Parse(match.Result("${Subversion}"), NumberStyles.AllowHexSpecifier);
                revitProduct.setSubversion(subversion);
                revitProduct.SetAllUsersAddInFolder(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                    "\\Autodesk\\Revit\\AddIns\\" + revitProduct.Version.ToString().Replace("Revit", ""));
                revitProduct.SetCurrentUserAddInFolder(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\Autodesk\\Revit\\AddIns\\" + revitProduct.Version.ToString().Replace("Revit", ""));
                return revitProduct;
            }

            return RevitProductUtility.GetInstalledProductInfo(productCode);
        }

        /// <summary> Is used to store all the RevitProduct which can be supported by this RevitAddInUtility.</summary>
        private static Hashtable _mProductsHashtable = null;

        private static UIntPtr _hkeyLocalMachine = new UIntPtr(2147483650u);

        private const int KeyRead = 131097;

        private const int KeyWow6464Key = 256;

        private const int KeyWow6432Key = 512;

        private const int GuidStringSize = 38;

        private const string RevitDBComponentID = "{DF7D485F-B8BA-448E-A444-E6FB1C258912}";

        private const int MaxSize = 260;

        private const string ReleaseSubVersion = "2021.0";
    }
}
