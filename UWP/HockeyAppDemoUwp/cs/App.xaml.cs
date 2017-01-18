//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.HockeyApp;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace HockeyAppDemo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // If you don't configure Hockey App in your App constructor, you need to configure it
            // at all entry points to the application: Activated, Launched, and Resuming (in the
            // case of a prelaunch, in which we deliberately don't configure Hockey App).
            //
            // The motivation behind not configuring HockeyApp in the App constructor is to avoid
            // a session start during a prelaunch. If you do not care about this in your app, you
            // may remove all calls to ConfigureHockeyApp and configure it in your App constructor.
 
            this.Resuming += App_Resuming;
            this.InitializeComponent();
            this.Construct();
        }

        private void App_Resuming(object sender, object e)
        {
            // If an app is opened after being prelaunched, OnLaunched is not called and
            // the Resuming event gets fired.
            ConfigureHockeyApp();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // An application opened by means other than normal user interaction is "activated,"
            // so Hockey App must be configured in this method
            ConfigureHockeyApp();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            BackgroundServices.MyBackgroundTask.Register();
            if (!e.PrelaunchActivated)
            {
                ConfigureHockeyApp();
                // Other Hockey App logic that needs to be in OnLaunched should go in here
                // to ensure that the HockeyClient has been configured.
            }
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private Frame CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;
                
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        // Add any application contructor code in here.
        partial void Construct();

        private bool HockeyAppConfigured = false;
        private void ConfigureHockeyApp()
        {
            if (HockeyAppConfigured)
            {
                return;
            }
            HockeyClient.Current.Configure(DemoConstants.YOUR_APP_ID,
                new TelemetryConfiguration() { EnableDiagnostics = true })
                .SetContactInfo("DemoUser", "demoapp@hotmail.com")
                .SetExceptionDescriptionLoader((Exception ex) =>
                {
                    return "Exception HResult: " + ex.HResult.ToString();
                });
            // Microsoft.HockeyApp.HockeyClient.Current.Configure(DemoConstants.YOUR_APP_ID);
            HockeyAppConfigured = true;
        }
    }
}
