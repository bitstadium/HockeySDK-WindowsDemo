using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HockeyApp;

namespace HockeyAppDemoWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            HockeyClient.Current.Configure(DemoConstants.YOUR_APP_ID);
            await HockeyClient.Current.SendCrashesAsync(true);
            await HockeyClient.Current.CheckForUpdatesAsync(true, () =>
            {
                if (Application.Current.MainWindow != null) { Application.Current.MainWindow.Close(); }
                return true;
            });
        }
    }
}
