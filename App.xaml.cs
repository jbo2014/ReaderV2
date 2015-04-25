using System;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;

using ReaderV2.Views;


namespace ReaderV2
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //currApp.StartupUri = new Uri("Windows1.xaml", UriKind.RelativeOrAbsolute);

            base.OnStartup(e);
            var mainWindow = new Window1Viewer(); // Create a new instance of MainWindow
            Application.Current.MainWindow = mainWindow; // Set mainWindow as current app's main window
            mainWindow.Show();
        }

        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);
        public static void DoEvents() 
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, exitFrameCallback, nestedFrame);
            Dispatcher.PushFrame(nestedFrame);
            if (exitOperation.Status != DispatcherOperationStatus.Completed) 
            {
                exitOperation.Abort();
            }
        }
        private static Object ExitFrame(Object state) 
        {
            DispatcherFrame frame = state as
            DispatcherFrame;
            frame.Continue = false;
            return null;
        }

    }
}
