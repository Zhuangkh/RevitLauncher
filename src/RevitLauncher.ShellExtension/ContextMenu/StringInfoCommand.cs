using System.Collections.Generic;
using RevitLauncher.Core;
using RevitLauncher.ShellExtension.ContextMenu.CommandBase;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu
{
    public class StringInfoCommand : BaseExplorerCommand
    {
        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_DEFAULT;
        public override string Icon { get; set; }

        private readonly string info;

        public StringInfoCommand(string info)
        {
            this.info = info;
        }

        public override EXPCMDSTATE GetState(IEnumerable<string> selectedFiles) => EXPCMDSTATE.ECS_DISABLED;

        public override string GetTitle(IEnumerable<string> selectedFiles) => info;

        public override string GetToolTip(IEnumerable<string> selectedFiles) => info;

        public override void Invoke(IEnumerable<string> selectedFiles) { }
    }
}
