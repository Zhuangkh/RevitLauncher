using System;

namespace RevitLauncher.ShellExtension.Shell32
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/ne-shobjidl_core-_expcmdstate
    /// </summary>
    [Flags]
    public enum EXPCMDSTATE
    {
        ECS_ENABLED = 0x00,
        ECS_DISABLED = 0x01,
        ECS_HIDDEN = 0x02,
        ECS_CHECKBOX = 0x04,
        ECS_CHECKED = 0x08,
        ECS_RADIOCHECK = 0x10,
    }
}
