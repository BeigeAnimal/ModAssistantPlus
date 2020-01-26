using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using ModAssistantPlus;

namespace ModAssistantPlus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string BeatSaberInstallDirectory;
        public static string BeatSaberInstallType;
        public static bool SaveModSelection;
        public static bool CheckInstalledMods;
        public static bool SelectInstalledMods;
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static List<string> SavedMods = ModAssistantPlus.Properties.Settings.Default.SavedMods.Split(',').ToList();


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (ModAssistantPlus.Properties.Settings.Default.UpgradeRequired)
            {
                ModAssistantPlus.Properties.Settings.Default.Upgrade();
                ModAssistantPlus.Properties.Settings.Default.UpgradeRequired = false;
                ModAssistantPlus.Properties.Settings.Default.Save();
            }

            Version = Version.Substring(0, Version.Length - 2);
            BeatSaberInstallDirectory = Utils.GetInstallDir();

            while (String.IsNullOrEmpty(App.BeatSaberInstallDirectory))
            {
                if (System.Windows.Forms.MessageBox.Show($"Press OK to try again, or Cancel to close application.", $"Couldn't find your Beat Saber install folder!", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    App.BeatSaberInstallDirectory = Utils.GetManualDir();
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            BeatSaberInstallType = ModAssistantPlus.Properties.Settings.Default.StoreType;
            SaveModSelection = ModAssistantPlus.Properties.Settings.Default.SaveSelected;
            CheckInstalledMods = ModAssistantPlus.Properties.Settings.Default.CheckInstalled;
            SelectInstalledMods = ModAssistantPlus.Properties.Settings.Default.SelectInstalled;

            if (e.Args.Length == 0)
            {
                Updater.Run();

                MainWindow window = new MainWindow();
                window.Show();
            }
            else
            {
                ArgumentHandler(e.Args);
            }
        }

        private void ArgumentHandler(string[] Args)
        {
            switch (Args[0])
            {
                case "--install":
                    if (!String.IsNullOrEmpty(Args[1]))
                        OneClickInstaller.InstallAsset(Args[1]);
                    else
                        Utils.SendNotify("Invalid argument! '--install' requires an option.");
                    break;

                case "--no-update":
                    MainWindow window = new MainWindow();
                    window.Show();
                    break;

                case "--register":
                    if (!String.IsNullOrEmpty(Args[1]))
                        OneClickInstaller.Register(Args[1], true);
                    else
                        Utils.SendNotify("Invalid argument! '--register' requires an option.");
                    break;

                case "--unregister":
                    if (!String.IsNullOrEmpty(Args[1]))
                        OneClickInstaller.Unregister(Args[1], true);
                    else
                        Utils.SendNotify("Invalid argument! '--unregister' requires an option.");
                    break;

                default:
                    Utils.SendNotify("Unrecognized argument. Closing Mod Assistant.");
                    break;
            }
            Current.Shutdown();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
            Application.Current.Shutdown();
        }
    }
}
