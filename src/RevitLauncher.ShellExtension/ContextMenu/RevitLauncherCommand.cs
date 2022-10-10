using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using RevitLauncher.Core;
using RevitLauncher.Core.Utils;
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
                var file = selectedFiles.First();
                var subCommands = new List<BaseExplorerCommand>();
                var info = new RevitFileInfo(file);
                subCommands.Add(new StringInfoCommand($"Revit Version: {info.SavedInVersion}"));

                var lockCommands = GetLockingProcess(file);
                if (lockCommands.Any())
                {
                    subCommands.Add(new StringInfoCommand("Locking Process"));
                    subCommands.AddRange(lockCommands);
                }

                var existCommands = GetExistProcess(file, info.SavedInVersion);
                if (existCommands.Any())
                {
                    subCommands.AddRange(existCommands);
                }

                var newCommands = GetNewProcess(file);
                if (newCommands.Any())
                {
                    subCommands.Add(new StringInfoCommand("Open In New Process"));
                    subCommands.AddRange(newCommands);
                }

                SubCommands = subCommands;
                return EXPCMDSTATE.ECS_ENABLED;
            }

            return EXPCMDSTATE.ECS_HIDDEN;
        }

        public override string GetTitle(IEnumerable<string> selectedFiles)
        {
            return $"RevitLauncher";
        }
        private static bool IsRevitFile(string path) => Path.GetExtension(path) == ".rvt" || Path.GetExtension(path) == ".rfa" || Path.GetExtension(path) == ".rft" || Path.GetExtension(path) == ".rte";
        public override string GetToolTip(IEnumerable<string> selectedFiles)
        {
            return "RevitLauncher";
        }

        public override void Invoke(IEnumerable<string> selectedFiles) { }

        private IEnumerable<BaseExplorerCommand> GetLockingProcess(string file)
        {
            var lockingProcesses = ProcessUtils.GetLockingProcesses(file);
            return lockingProcesses.Select(x => new LockingProcessCommand(x));
        }

        private IEnumerable<BaseExplorerCommand> GetExistProcess(string file, string version)
        {
            var processes = Process.GetProcesses().Where(x => x.ProcessName == ("Revit"));
            var subCommands = new List<BaseExplorerCommand>();
            if (processes.Any())
            {
                List<BaseExplorerCommand> matchCommand = new List<BaseExplorerCommand>();
                List<BaseExplorerCommand> mismatchCommand = new List<BaseExplorerCommand>();
                foreach (Process process in processes)
                {
                    string title = string.IsNullOrEmpty(process.MainWindowTitle)
                                    ? Win32API.GetAllDesktopWindows().First(x => x.Pid.ToInt32() == process.Id).WindowName
                                    : process.MainWindowTitle;
                    string[] titles = process.MainWindowTitle.Split('-');
                    title = title.StartsWith("Autodesk Revit")
                            ? title.Substring(14).Trim()
                            : title;

                    if (titles[0].Contains(version))
                    {
                        matchCommand.Add(new ExistProcessCommand(file, process));
                    }
                    else
                    {
                        mismatchCommand.Add(new ExistProcessCommand(file, process));
                    }
                }

                if (matchCommand.Any())
                {
                    subCommands.Add(new StringInfoCommand("Match Vesion Process"));
                    subCommands.AddRange(matchCommand);
                }

                if (mismatchCommand.Any())
                {
                    subCommands.Add(new StringInfoCommand("Mismatch Vesion Process"));
                    subCommands.AddRange(mismatchCommand);
                }
            }
            return subCommands;
        }

        private IEnumerable<BaseExplorerCommand> GetNewProcess(string file)
        {
            return RevitProductUtility.GetAllInstalledRevitProducts().Select(x => new NewProcessCommand(file, x));
        }
    }
}
