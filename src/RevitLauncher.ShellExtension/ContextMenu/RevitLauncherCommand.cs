using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RevitLauncher.ShellExtension.ContextMenu.CommandBase;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu
{
    [ComVisible(true)]
    [Guid("5444C4A1-D5CE-4126-A734-58836F177027")]

    public class RevitLauncherCommand : BaseExplorerCommand
    {
        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_HASSUBCOMMANDS;
        public override string Icon { get; set; } = Directory.GetParent(typeof(RevitLauncherCommand).Assembly.Location) + "\\launchericon.ico";

        public override EXPCMDSTATE GetState(IEnumerable<string> selectedFiles)
        {
            if (selectedFiles.Any(IsRevitFile) && selectedFiles.Count() == 1)
            {
                var subCommands = new List<BaseExplorerCommand>();
                subCommands.Add(new VerInfoCommand(selectedFiles.First()));
                SubCommands = subCommands;
                return EXPCMDSTATE.ECS_ENABLED;
            }

            return EXPCMDSTATE.ECS_ENABLED;
        }

        public override string GetTitle(IEnumerable<string> selectedFiles)
        {
            return $"RevitLauncher";
        }
        private static bool IsRevitFile(string path) => Path.GetExtension(path) == ".rvt" || Path.GetExtension(path) == ".rfa" || Path.GetExtension(path) == ".rft" || Path.GetExtension(path) == ".rte";
        public override string GetToolTip(IEnumerable<string> selectedFiles)
        {
            return "Revit";
        }

        public override void Invoke(IEnumerable<string> selectedFiles)
        {

        }
    }
}
