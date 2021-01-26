using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace InstallerAction
{
    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        private const string AssemblyName = "RevitLauncher.dll";
        private readonly string _installScript = $"install {AssemblyName} -codebase";
        private readonly string _uninstallScript = $"uninstall {AssemblyName}";
        private string path => Path.GetDirectoryName(Context.Parameters["assemblypath"]);

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            Process myprocess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo($"{path}\\srm.exe", _installScript);
            myprocess.StartInfo = startInfo;
            myprocess.StartInfo.UseShellExecute = false;
            myprocess.Start();

        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            Process myprocess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo($"{path}\\srm.exe", _uninstallScript);
            myprocess.StartInfo = startInfo;
            myprocess.StartInfo.UseShellExecute = false;
            myprocess.Start();
        }

    }
}
