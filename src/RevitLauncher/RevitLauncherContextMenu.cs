using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using RevitLauncher.Utils;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace RevitLauncher
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".rvt", ".rte", ".rft", ".rfa")]
    public class RevitLauncherContextMenu : SharpContextMenu
    {
        private ContextMenuStrip menu = new ContextMenuStrip();

        /// <summary>
        /// 检测ContextMenu可用时更新ContextMenu
        /// </summary>
        /// <returns></returns>
        protected override bool CanShowMenu()
        {
            if (SelectedItemPaths.Count() == 1)
            {
                this.Update();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新ContextMenu菜单
        /// </summary>
        private void Update()
        {
            menu.Dispose();
            menu = CreateMenu();
        }

        /// <summary>
        /// 创建ContextMenu
        /// </summary>
        /// <returns></returns>
        protected override ContextMenuStrip CreateMenu()
        {
            menu.Items.Clear();

            #region 主菜单
            var launcherItem = new ToolStripMenuItem()
            {
                Text = "Revit Launcher",
                Image = Image.FromFile($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\launchericon.ico")
            };
            #endregion

            #region 版本信息
            var info = new RevitFileInfo(SelectedItemPaths.First());
            var verItem = new ToolStripMenuItem()
            {
                Text = $"Revit Version: {info.SavedInVersion}",
                Enabled = false
            };
            #endregion

            #region Locking Processes
            List<ToolStripItem> lockingProcessesMenuItems = new List<ToolStripItem>();
            var lockingProcesses = ProcessUtils.GetLockingProcesses(SelectedItemPaths.First());
            foreach (Process lockingProcess in lockingProcesses)
            {
                string title = string.IsNullOrEmpty(lockingProcess.MainWindowTitle)
                    ? Win32API.GetAllDesktopWindows().First(x => x.Pid.ToInt32() == lockingProcess.Id).WindowName
                    : lockingProcess.MainWindowTitle;
                var processItem = new ToolStripMenuItem()
                {
                    Text = title,
                    Tag = lockingProcess
                };
                processItem.Click += LockingProcessItem_Click;
                lockingProcessesMenuItems.Add(processItem);
            }
            #endregion

            #region 当前存在进程菜单

            List<ToolStripItem> processMenuItemsMatch = new List<ToolStripItem>();
            List<ToolStripItem> processMenuItemsNotMatch = new List<ToolStripItem>();

            var processes = Process.GetProcesses().Where(x => x.ProcessName == ("Revit"));
            foreach (Process process in processes)
            {
                string title = string.IsNullOrEmpty(process.MainWindowTitle)
                                ? Win32API.GetAllDesktopWindows().First(x => x.Pid.ToInt32() == process.Id).WindowName
                                : process.MainWindowTitle;
                string[] titles = process.MainWindowTitle.Split('-');
                title = title.StartsWith("Autodesk Revit")
                        ? title.Substring(14).Trim()
                        : title;

                var processItem = new ToolStripMenuItem()
                {
                    Text = title,
                    Tag = process.Id
                };
                processItem.Click += CurProcessItem_Click;
                if (titles[0].Contains(info.SavedInVersion))
                {
                    processMenuItemsMatch.Add(processItem);
                }
                else
                {
                    processMenuItemsNotMatch.Add(processItem);
                }
            }
            #endregion
            
            #region 新进程菜单
            var curItem = new ToolStripMenuItem()
            {
                Text = $"对应版本新进程中打开",
            };
            var manualItem = new ToolStripMenuItem()
            {
                Text = $"指定版本新窗口中新开",
            };

            foreach (RevitProduct product in RevitProductUtility.GetAllInstalledRevitProducts())
            {
                var processItem = new ToolStripMenuItem()
                {
                    Text = product.Name,
                    Tag = product.InstallLocation
                };

                if (product.Version.ToString().Contains(info.SavedInVersion))
                {
                    curItem.Tag = product.InstallLocation;
                }

                processItem.Click += NewProcess_Click;
                manualItem.DropDownItems.Add(processItem);
            }

            if (curItem.Tag == null)
            {
                curItem.Enabled = false;
                curItem.Text += "[无对应版本]";
            }
            else
            {
                curItem.Click += NewProcess_Click;
            }
            #endregion

            launcherItem.DropDownItems.Add(verItem);
            launcherItem.DropDownItems.Add(new ToolStripSeparator());
            if (lockingProcesses.Any())
            {
                launcherItem.DropDownItems.Add(new ToolStripMenuItem()
                {
                    Text = $"File Locking Processes",
                    Enabled = false
                });
                foreach (ToolStripItem item in lockingProcessesMenuItems)
                {
                    launcherItem.DropDownItems.Add(item);
}
                launcherItem.DropDownItems.Add(new ToolStripSeparator());
            }
            if (processMenuItemsMatch.Any())
            {
                launcherItem.DropDownItems.Add(new ToolStripMenuItem()
                {
                    Text = $"版本匹配进程",
                    Enabled = false
                });
                foreach (ToolStripItem item in processMenuItemsMatch)
                {
                    launcherItem.DropDownItems.Add(item);
                }
                launcherItem.DropDownItems.Add(new ToolStripSeparator());
            }
            if (processMenuItemsNotMatch.Any())
            {
                launcherItem.DropDownItems.Add(new ToolStripMenuItem()
                {
                    Text = $"版本不匹配进程",
                    Enabled = false
                });
                foreach (ToolStripItem item in processMenuItemsNotMatch)
                {
                    launcherItem.DropDownItems.Add(item);
                }
                launcherItem.DropDownItems.Add(new ToolStripSeparator());
            }
            launcherItem.DropDownItems.Add(curItem);
            launcherItem.DropDownItems.Add(manualItem);

            menu.AllowDrop = true;
            menu.Items.Add(launcherItem);

            return menu;
        }

        /// <summary>
        /// 当前进程中打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurProcessItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (NamedPipeClientStream client = new NamedPipeClientStream(".", (sender as ToolStripMenuItem).Tag.ToString(), PipeDirection.Out))
                {
                    client.Connect(1000);
                    StreamWriter streamWriter = new StreamWriter(client) { AutoFlush = true };
                    streamWriter.Write(SelectedItemPaths.First());
                    client.WaitForPipeDrain();
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// 当前进程中打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LockingProcessItem_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Tag is Process process)
            {
                ProcessUtils.SwitchToProcess(process);
            }
        }

        /// <summary>
        /// 新进程打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProcess_Click(object sender, System.EventArgs e)
        {
            ProcessUtils.StartProcess($"{(sender as ToolStripMenuItem).Tag}Revit.exe", SelectedItemPaths.First());
        }
    }
}
