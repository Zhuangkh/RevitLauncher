using System.Collections.Generic;
using RevitLauncher.ShellExtension.Interop.Shell32;

namespace RevitLauncher.ShellExtension.ContextMenu.CommandBase
{
    public static class ShellExtensionHelper
    {
        public static IEnumerable<string> GetFilePaths(this IShellItemArray itemArray)
        {
            var count = itemArray.GetCount();
            string[] paths = new string[count];

            for (int i = 0; i < count; i++)
            {
                IShellItem item = itemArray.GetItemAt((uint)i);
                item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out var name);
                paths[i] = name;
            }

            return paths;
        }
    }
}
