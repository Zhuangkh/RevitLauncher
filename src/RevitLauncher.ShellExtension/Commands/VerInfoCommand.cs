using System;
using System.Collections.Generic;
using System.IO;
using RevitLauncher.Core;
using RevitLauncher.ShellExtension.Shell32;

namespace RevitLauncher.ShellExtension
{
    public class VerInfoCommand : BaseExplorerCommand
    {
        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_DEFAULT;

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
