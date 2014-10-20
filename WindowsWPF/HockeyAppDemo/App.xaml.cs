using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HockeyApp;
using System.Diagnostics;

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

            #region HOCKEYAPP SAMPLE CODE

            //main configuration of HockeySDK
            HockeyClient.Current.Configure(DemoConstants.YOUR_APP_ID)
                .UseCustomResourceManager(HockeyApp.ResourceManager) //register your own resourcemanager to override HockeySDK i18n strings
                //.RegisterCustomUnhandledExceptionLogic((eArgs) => { /* do something here */ }) // define a callback that is called after unhandled exception
                //.RegisterCustomUnobserveredTaskExceptionLogic((eArgs) => { /* do something here */ }) // define a callback that is called after unobserved task exception
                //.RegisterCustomDispatcherUnhandledExceptionLogic((args) => { }) // define a callback that is called after dispatcher unhandled exception
                //.SetApiDomain("https://your.hockeyapp.server")
                .SetContactInfo("John Smith", "email@example.com");

            //optional should only used in debug builds. register an event-handler to get exceptions in HockeySDK code that are "swallowed" (like problems writing crashlogs etc.)
#if DEBUG
((HockeyClient)HockeyClient.Current).OnHockeySDKInternalException += (sender, args) =>
            {
                if (Debugger.IsAttached) { Debugger.Break(); }
            };
#endif
            //send crashes to the HockeyApp server
            await HockeyClient.Current.SendCrashesAsync();

            //check for updates on the HockeyApp server
            await HockeyClient.Current.CheckForUpdatesAsync(true, () =>
            {
                if (Application.Current.MainWindow != null) { Application.Current.MainWindow.Close(); }
                return true;
            });

            #endregion
        }
    }
}
