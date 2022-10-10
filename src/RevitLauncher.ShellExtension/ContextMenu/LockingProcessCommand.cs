using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RevitLauncher.Core.Utils;
using RevitLauncher.ShellExtension.ContextMenu.CommandBase;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu
{
    public class LockingProcessCommand : BaseExplorerCommand
    {
        private readonly Process process;
        private readonly string title;

        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_DEFAULT;
        public override string Icon { get; set; }

        public LockingProcessCommand(Process process)
        {
            this.process = process;
            this.title = string.IsNullOrEmpty(process.MainWindowTitle)
                   ? Win32API.GetAllDesktopWindows().First(x => x.Pid.ToInt32() == process.Id).WindowName
                   : process.MainWindowTitle;
            this.Icon = process.MainModule.FileName;
        }

        public override EXPCMDSTATE GetState(IEnumerable<string> selectedFiles)
        {
            return EXPCMDSTATE.ECS_ENABLED;
        }

        public override string GetTitle(IEnumerable<string> selectedFiles)
        {
            return title;
        }

        public override string GetToolTip(IEnumerable<string> selectedFiles)
        {
            return title;
        }

        public override void Invoke(IEnumerable<string> selectedFiles)
        {
            ProcessUtils.SwitchToProcess(process);
        }
    }

    public class NewProcessCommand : BaseExplorerCommand
    {

        private readonly string file;
        private readonly RevitProduct application;
        private string title;

        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_DEFAULT;
        public override string Icon { get; set; }

        public NewProcessCommand(string file, RevitProduct application)
        {
            this.application = application;
            this.Icon = $"{application.InstallLocation}Revit.exe";
            this.title = application.Name;
            this.file = file;
        }

        public override EXPCMDSTATE GetState(IEnumerable<string> selectedFiles)
        {
            return EXPCMDSTATE.ECS_ENABLED;
        }

        public override string GetTitle(IEnumerable<string> selectedFiles)
        {
            return title;
        }

        public override string GetToolTip(IEnumerable<string> selectedFiles)
        {
            return title;
        }

        public override void Invoke(IEnumerable<string> selectedFiles)
        {
            ProcessUtils.StartProcess($"{application.InstallLocation}Revit.exe", file);
        }
    }
}
