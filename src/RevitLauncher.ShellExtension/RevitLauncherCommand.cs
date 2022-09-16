using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using RevitLauncher.ShellExtension.Shell32;

namespace RevitLauncher.ShellExtension
{
    [ComVisible(true)]
    [Guid("5444C4A1-D5CE-4126-A734-58836F177027")]

    public class RevitLauncherCommand : BaseExplorerCommand
    {
        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_HASSUBCOMMANDS;

        public bool flag { get; set; } = false;
        public override EXPCMDSTATE GetState(IEnumerable<string> selectedFiles)
        {
            return selectedFiles.Any(IsRevitFile) ? EXPCMDSTATE.ECS_ENABLED : EXPCMDSTATE.ECS_HIDDEN;
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
