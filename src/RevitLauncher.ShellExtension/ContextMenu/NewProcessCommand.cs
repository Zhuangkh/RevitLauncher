using System.Collections.Generic;
using RevitLauncher.Core.Utils;
using RevitLauncher.ShellExtension.ContextMenu.CommandBase;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu
{
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
