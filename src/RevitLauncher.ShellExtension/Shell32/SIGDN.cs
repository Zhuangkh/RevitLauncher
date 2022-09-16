﻿namespace RevitLauncher.ShellExtension.Shell32
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/ne-shobjidl_core-sigdn
    /// </summary>
    public enum SIGDN : uint
    {
        SIGDN_NORMALDISPLAY = 0,
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
        SIGDN_FILESYSPATH = 0x80058000,
        SIGDN_URL = 0x80068000,
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
        SIGDN_PARENTRELATIVE = 0x80080001,
        SIGDN_PARENTRELATIVEFORUI = 0x80094001,
    }
}
