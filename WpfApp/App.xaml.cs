using System;
using System.Windows;

namespace WpfApp {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            Console.WriteLine("WpfApp starting up on .NET Framework 4.7.1");

            // Determine mode from command-line args. Supported flags:
            //  --ci  : run headless WPF load/instantiation checks and exit with 0/1
            //  --gui : start the GUI (show MainWindow)
            // Default: GUI mode.
            var argsList = e.Args ?? new string[0];
            var isCi = Array.Exists(argsList, a => string.Equals(a, "--ci", StringComparison.OrdinalIgnoreCase));
            var isGui = Array.Exists(argsList, a => string.Equals(a, "--gui", StringComparison.OrdinalIgnoreCase));

            // Also detect common CI environment variables so that the app won't
            // accidentally show UI on CI runners if callers forget to pass --ci.
            // GitHub Actions sets GITHUB_ACTIONS=true; many CI systems set CI=true.
            try {
                var gh = Environment.GetEnvironmentVariable("GITHUB_ACTIONS");
                var ciEnv = Environment.GetEnvironmentVariable("CI");
                if (!isCi && (string.Equals(gh, "true", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(ciEnv, "true", StringComparison.OrdinalIgnoreCase))) {
                    isCi = true;
                    Console.WriteLine("Detected CI environment via environment variables; enabling headless --ci mode.");
                }
            } catch (Exception) {
                // Ignore environment access errors and proceed with explicit args.
            }

            if (isCi && isGui) {
                Console.WriteLine("Both --ci and --gui specified; --ci takes precedence.");
            }

            if (isCi) {
                // Headless CI check: verify WPF types can be loaded and instantiated.
                var success = true;
                try {
                    var tWindow = typeof(System.Windows.Window);
                    var tButton = typeof(System.Windows.Controls.Button);
                    var tDispatcher = typeof(System.Windows.Threading.Dispatcher);

                    Console.WriteLine($"Type check: {tWindow.FullName} from {tWindow.Assembly.GetName()}");
                    Console.WriteLine($"Type check: {tButton.FullName} from {tButton.Assembly.GetName()}");
                    Console.WriteLine($"Type check: {tDispatcher.FullName} from {tDispatcher.Assembly.GetName()}");

                    var w = new System.Windows.Window();
                    var b = new System.Windows.Controls.Button();
                    b.Content = "probe";

                    Console.WriteLine("WPF types instantiated successfully (headless check).");
                } catch (Exception ex) {
                    Console.WriteLine("WPF load/instantiation failed: " + ex);
                    success = false;
                }

                Shutdown(success ? 0 : 1);
                return;
            }

            // GUI mode (default): show main window.
            Console.WriteLine("Starting GUI mode (showing MainWindow)");
            var main = new MainWindow();
            main.Show();
        }
    }
}
