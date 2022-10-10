using System.Collections.Generic;
using RevitLauncher.Core;
using RevitLauncher.ShellExtension.ContextMenu.CommandBase;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu
{
    public class VerInfoCommand : BaseExplorerCommand
    {
        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_DEFAULT;
        public override string Icon { get; set; }

        private string version;

        public VerInfoCommand(string path)
        {
            var info = new RevitFileInfo(path);
            version = $"Revit Version: {info.SavedInVersion}";
        }

        public override EXPCMDSTATE GetState(IEnumerable<string> selectedFiles) => EXPCMDSTATE.ECS_DISABLED;

        public override string GetTitle(IEnumerable<string> selectedFiles) => version;

        public override string GetToolTip(IEnumerable<string> selectedFiles) => version;

        public override void Invoke(IEnumerable<string> selectedFiles) { }
    }
}
