using System;
using System.Collections.Generic;
using System.Linq;
using RevitLauncher.ShellExtension.Interop;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu.CommandBase
{
    public abstract class BaseExplorerCommand : IExplorerCommand
    {
        public virtual Guid CanonicalName => GetType().GUID;

        public abstract EXPCMDFLAGS Flags { get; set; }

        public virtual IEnumerable<BaseExplorerCommand> SubCommands { get; set; } = Array.Empty<BaseExplorerCommand>();

        public abstract string GetTitle(IEnumerable<string> selectedFiles);

        public abstract string GetToolTip(IEnumerable<string> selectedFiles);

        public abstract EXPCMDSTATE GetState(IEnumerable<string> selectedFiles);

        public abstract void Invoke(IEnumerable<string> selectedFiles);

        void IExplorerCommand.GetTitle(IShellItemArray itemArray, out string title)
        {
            title = GetTitle(itemArray.GetFilePaths());
        }

        void IExplorerCommand.GetIcon(IShellItemArray itemArray, out string resourceString)
        {
            throw new NotImplementedException();
        }

        void IExplorerCommand.GetToolTip(IShellItemArray itemArray, out string tooltip)
        {
            tooltip = GetToolTip(itemArray.GetFilePaths());
        }

        void IExplorerCommand.GetCanonicalName(out Guid guid)
        {
            guid = CanonicalName;
        }

        void IExplorerCommand.GetState(IShellItemArray itemArray, bool okToBeShow, out EXPCMDSTATE commandState)
        {
            commandState = GetState(itemArray.GetFilePaths());
        }

        void IExplorerCommand.Invoke(IShellItemArray itemArray, object bindCtx)
        {
            Invoke(itemArray.GetFilePaths());
        }

        void IExplorerCommand.GetFlags(out EXPCMDFLAGS flags)
        {
            flags = Flags;
        }

        HRESULT IExplorerCommand.EnumSubCommands(out IEnumExplorerCommand commandEnum)
        {
            IEnumerable<BaseExplorerCommand> subcommands = SubCommands;

            if (subcommands != null && subcommands.Any())
            {
                commandEnum = new EnumExplorerCommand(subcommands);
                return HRESULT.S_OK;
            }
            else
            {
                commandEnum = null;
                return HRESULT.S_FALSE;
            }
        }
    }
}
