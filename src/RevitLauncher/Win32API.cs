using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitLauncher
{
    public class Win32API
    {
        internal delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        /// <summary>
        /// 遍历所有窗口
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        
        internal static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        /// <summary>
        /// 获取窗口Text
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 获取窗口类名
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 通过Window Handle获取pid
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out IntPtr processId);

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

	}
}
