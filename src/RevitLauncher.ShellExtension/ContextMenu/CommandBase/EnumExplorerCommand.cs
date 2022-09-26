using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RevitLauncher.ShellExtension.Interop;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu.CommandBase
{
    public class EnumExplorerCommand : IEnumExplorerCommand
    {
        private readonly IExplorerCommand[] commands;
        private uint index;

        public EnumExplorerCommand(IEnumerable<IExplorerCommand> commands)
        {
            this.commands = commands.ToArray();
        }

        public HRESULT Next(uint celt, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface, SizeParamIndex = 0)] IExplorerCommand[] pUICommand, out uint pceltFetched)
        {
            var hr = HRESULT.S_FALSE;
            pceltFetched = 0;
            if (index <= commands.Length)
            {
                uint uIndex = 0;
                while (uIndex < celt && index < commands.Length)
                {
                    pUICommand[uIndex] = commands[index];
                    uIndex++;
                    index++;
                }

                pceltFetched = uIndex;

                if (uIndex == celt)
                {
                    hr = HRESULT.S_OK;
                }
            }
            return hr;
        }

        public HRESULT Skip(uint count)
        {
            index += count;
            if (index > commands.Length)
            {
                index = (uint)commands.Length;
                return HRESULT.S_FALSE;
            }
            return HRESULT.S_OK;
        }

        public void Reset()
        {
            index = 0;
        }

        public IEnumExplorerCommand Clone()
        {
            EnumExplorerCommand copy = new EnumExplorerCommand(commands);
            copy.index = index;
            return copy;
        }
    }
}
