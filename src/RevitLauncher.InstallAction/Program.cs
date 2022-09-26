using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RevitLauncher.InstallAction
{
    internal class Program
    {
        [DllImport("propsys.dll", SetLastError = false, ExactSpelling = true)]
        private static extern uint PSRegisterPropertySchema([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

        [DllImport("propsys.dll", SetLastError = false, ExactSpelling = true)]
        private static extern uint PSUnregisterPropertySchema([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

        [DllImport("propsys.dll", SetLastError = false, ExactSpelling = true)]
        private static extern uint PSRefreshPropertySchema();

        private const string Propdesc = "RevitLauncher.propdesc";
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string path = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + $"\\{Propdesc}";
                switch (args[0])
                {
                    case "-i":
                        Console.WriteLine(PSRegisterPropertySchema(path).ToString());
                        Console.WriteLine(PSRefreshPropertySchema().ToString());
                        Register();
                        break;
                    case "-u":
                        Unregister();
                        Console.WriteLine(PSUnregisterPropertySchema(path));
                        Console.WriteLine(PSRefreshPropertySchema().ToString());
                        break;
                }
            }


        }

        static void Register()
        {
            var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PropertySystem\\PropertyHandlers\\", true);
            key.CreateSubKey(".rte", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.CreateSubKey(".rvt", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.CreateSubKey(".rft", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.CreateSubKey(".rfa", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.OpenSubKey(".rte", true).SetValue("", "{6542121F-1D79-4A27-9471-F80277DA8535}");
            key.OpenSubKey(".rvt", true).SetValue("", "{6542121F-1D79-4A27-9471-F80277DA8535}");
            key.OpenSubKey(".rft", true).SetValue("", "{6542121F-1D79-4A27-9471-F80277DA8535}");
            key.OpenSubKey(".rfa", true).SetValue("", "{6542121F-1D79-4A27-9471-F80277DA8535}");
            key = Registry.ClassesRoot.CreateSubKey("CLSID\\{6542121F-1D79-4A27-9471-F80277DA8535}", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key = Registry.ClassesRoot.OpenSubKey("CLSID\\{6542121F-1D79-4A27-9471-F80277DA8535}", true);
            key.SetValue("", "Revit Property Handler");
            key.SetValue("ManualSafeSave ", 1);
            key.CreateSubKey("InProcServer32", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.OpenSubKey("InProcServer32", true).SetValue("", new FileInfo(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"..\RevitLauncher.ShellExtension\RevitLauncher.ShellExtension.comhost.dll").FullName);
            key.OpenSubKey("InProcServer32", true).SetValue("ThreadingModel", "Apartment");
        }

        static void Unregister()
        {
            var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PropertySystem\\PropertyHandlers\\", true);
            key.DeleteSubKey(".rte", false);
            key.DeleteSubKey(".rvt", false);
            key.DeleteSubKey(".rft", false);
            key.DeleteSubKey(".rfa", false);
            Registry.ClassesRoot.DeleteSubKey("CLSID\\{6542121F-1D79-4A27-9471-F80277DA8535}", false);
        }
    }
}