using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using RevitLauncher.Core.Utils;
using RevitLauncher.ShellExtension.ContextMenu.CommandBase;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu
{
    public class ExistProcessCommand : BaseExplorerCommand
    {
        private readonly string file;
        private readonly Process process;
        private readonly string title;

        public override EXPCMDFLAGS Flags { get; set; } = EXPCMDFLAGS.ECF_DEFAULT;
        public override string Icon { get; set; }

        public ExistProcessCommand(string file, Process process)
        {
            this.file = file;
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
            try
            {
                using (NamedPipeClientStream client = new NamedPipeClientStream(".", process.Id.ToString(), PipeDirection.Out))
                {
                    client.Connect(1000);
                    StreamWriter streamWriter = new StreamWriter(client) { AutoFlush = true };
                    streamWriter.Write(file);
                    client.WaitForPipeDrain();
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }
    }
}
