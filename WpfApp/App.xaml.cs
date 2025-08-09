using System;
using System.Windows;

namespace WpfApp {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            Console.WriteLine("WpfApp starting up on .NET Framework 4.7.1");
            // Exit immediately to allow CI to run the exe headlessly.
            Shutdown(0);
        }
    }
}
