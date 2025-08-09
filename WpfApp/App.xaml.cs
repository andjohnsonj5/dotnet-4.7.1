using System;
using System.Windows;

namespace WpfApp {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            Console.WriteLine("WpfApp starting up on .NET Framework 4.7.1");

            // Verify that key WPF assemblies and types can be loaded and instantiated.
            // This is a lightweight, headless check suitable for CI to validate
            // that WPF-related class libraries are available on the runtime.
            var success = true;
            try {
                // Touch several framework types and instantiate simple controls.
                var tWindow = typeof(System.Windows.Window);
                var tButton = typeof(System.Windows.Controls.Button);
                var tDispatcher = typeof(System.Windows.Threading.Dispatcher);

                Console.WriteLine($"Type check: {tWindow.FullName} from {tWindow.Assembly.GetName()}");
                Console.WriteLine($"Type check: {tButton.FullName} from {tButton.Assembly.GetName()}");
                Console.WriteLine($"Type check: {tDispatcher.FullName} from {tDispatcher.Assembly.GetName()}");

                // Instantiate simple objects (do not show any UI).
                var w = new System.Windows.Window();
                var b = new System.Windows.Controls.Button();
                // Access a property to ensure types are usable.
                b.Content = "probe";

                Console.WriteLine("WPF types instantiated successfully (headless check).");
            } catch (Exception ex) {
                Console.WriteLine("WPF load/instantiation failed: " + ex);
                success = false;
            }

            // Exit with non-zero code on failure so CI can detect problems.
            Shutdown(success ? 0 : 1);
        }
    }
}
