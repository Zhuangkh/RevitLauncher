using System;
using System.Diagnostics;

namespace RevitLauncher
{
    public class ProcessUtils
    {
        ///   <summary>   
        ///   启动其他的应用程序   
        ///   </summary>   
        ///   <param name="file">应用程序名称</param>     
        ///   <param name="args">命令行参数</param>   
        public static bool StartProcess(string exe, string args)
        {
            try
            {
                Process myprocess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(exe, $"\"{args}\"");
                myprocess.StartInfo = startInfo;
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();
                return true;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }
    }
}