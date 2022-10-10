using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RevitLauncher.Core.Utils
{
    public class Win32API
    {
        internal delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        
        internal static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        internal static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out IntPtr processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr SetFocus(HandleRef hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        internal static extern int RmRegisterResources(uint pSessionHandle,
                                              UInt32 nFiles,
                                              string[] rgsFilenames,
                                              UInt32 nApplications,
                                              [In] RM_UNIQUE_PROCESS[] rgApplications,
                                              UInt32 nServices,
                                              string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        internal static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        internal static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        internal static extern int RmGetList(uint dwSessionHandle,
                                    out uint pnProcInfoNeeded,
                                    ref uint pnProcInfo,
                                    [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
                                    ref uint lpdwRebootReasons);
        [DllImport("user32.dll")]
        internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegOpenKeyEx(UIntPtr hKey, string subKey, int ulOptions, int samDesired,
            out UIntPtr hkResult);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName, int lpReserved, out uint lpType,
            StringBuilder lpData, out uint lpcbData);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName, int lpReserved, out uint lpType,
            out uint lpData, out uint lpcbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int RegCloseKey(UIntPtr hKey);

        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        internal static extern uint MsiEnumClients(string szComponent, uint iProductIndex, string lpProductBuf);

        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        internal static extern int MsiGetComponentPath(string szProduct, string szComponent, string lpPathBuf,
            out uint pcchBuf);

        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        internal static extern int MsiGetProductInfo(string product, string property, string valueBuf, out int len);

        public struct WindowInfo
        {
            public IntPtr HWnd;
            public IntPtr Pid;
            public string WindowName;
            public string ClassName;
        }

        public static WindowInfo[] GetAllDesktopWindows()
        {
            List<WindowInfo> wndList = new List<WindowInfo>();

            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);

                wnd.HWnd = hWnd;
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.WindowName = sb.ToString();
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.ClassName = sb.ToString();
                GetWindowThreadProcessId(hWnd, out wnd.Pid);

                wndList.Add(wnd);
                return true;
            }, 0);

            return wndList.ToArray();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        internal const int RmRebootReasonNone = 0;
        internal const int CCH_RM_MAX_APP_NAME = 255;
        internal const int CCH_RM_MAX_SVC_NAME = 63;

        internal enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;

            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

    }
}
