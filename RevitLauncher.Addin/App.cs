using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace RevitLauncher.Addin
{
    public class App : IExternalApplication
    {
        private NamedPipeServerStream _server;
        private string _pipeName;
        private ExternalEvent _externalEvent;
        public static string Path { get; set; } = string.Empty;

        public App()
        {
            _pipeName = Process.GetCurrentProcess().Id.ToString();
            _server = new NamedPipeServerStream(_pipeName, PipeDirection.In, 3, PipeTransmissionMode.Message);
        }

        public Result OnStartup(UIControlledApplication application)
        {
            _externalEvent = ExternalEvent.Create(new FileOpenExternalEvent());
            Task.Run(new Action(() =>
            {
                while (true)
                {
                    _server.WaitForConnection();
                    Trace.WriteLine("Conneted");

                    StreamReader streamReader = new StreamReader(_server);
                    string line = string.Empty;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        App.Path = line;
                        _externalEvent.Raise();
                    }

                    Trace.WriteLine("Disconnetion");
                    _server.Disconnect();

                    Task.Delay(100);
                }

            }));
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            _server?.Dispose();

            return Result.Succeeded;
        }
    }
}