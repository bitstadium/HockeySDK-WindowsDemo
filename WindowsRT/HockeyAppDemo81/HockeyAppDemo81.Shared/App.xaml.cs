using HockeyApp;
using HockeyApp.Common;
using MetroLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace HockeyAppDemo81
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {

        public App()
        {

            #region HOCKEYAPP SAMPLE CODE

            //this is just a sample in memory logger using MetroLog
            var config = new LoggingConfiguration();
            var inMemoryLogTarget = new InMemoryLogTarget(40);
            config.AddTarget(LogLevel.Warn, inMemoryLogTarget);
            LogManagerFactory.DefaultConfiguration = config;

            //main configuration method for HockeySDK. following lines are optional configurations options
            HockeyClient.Current.Configure(DemoConstants.YOUR_APP_ID)
                .SetExceptionDescriptionLoader((ex) => { return inMemoryLogTarget.LogLines.Aggregate((a, b) => a + "\n" + b); }) //return additional info from your logger on crash
                // .RegisterCustomUnhandledExceptionLogic((eArgs) => { return true; }) // define a callback that is called after unhandled exception. returnvalue indicates if application exit should be called
                // .RegisterCustomUnobserveredTaskExceptionLogic((eArgs) => { return false; }) // define a callback that is called after unobserved task exception. returnvalue indicates if application exit should be called
                // .SetApiDomain("https://your.dedicated.hockey.server")
                .SetContactInfo(DemoConstants.USER_NAME, DemoConstants.USER_EMAIL);

            //optional register your logger for internal logging of HockeySDK
            HockeyLogManager.GetLog = (t) => { return new HockeyAppMetroLogWrapper(t); };

            //optional should only used in debug builds. register an event-handler to get exceptions in HockeySDK code that are "swallowed" (like problems writing crashlogs etc.)
#if DEBUG
            ((HockeyClient)HockeyClient.Current).OnHockeySDKInternalException += (sender, args) => {
                if (Debugger.IsAttached)
                {
                    //Debugger.Break();
                }
            };
#endif
            #endregion

            #region Standard template code 
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            #endregion
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            #region Standard template code
            Frame rootFrame = CreateRootFrame();

#if WINDOWS_PHONE_APP
            await RestoreStatusAsync(e.PreviousExecutionState);
#endif
            // Ensure the current window is active
            Window.Current.Activate();
            #endregion

            #region HOCKEYAPP SAMPLE CODE

            //checks for existing crashlogs and sends them to the server (asking user with a dialog if not sendWithoutAsking)
            await HockeyClient.Current.SendCrashesAsync(/* sendWithoutAsking: true */);

            //initiate hockeyapp authentifictation 
            HockeyClient.Current.IdentifyUser(DemoConstants.YOUR_APP_SECRET, 
                typeof(MainPage), 
                //tokenValidationPolicy: TokenValidationPolicy.OnNewVersion,
                authValidationMode: AuthValidationMode.Graceful
                );

            //following line commented out because we want to go to the main page only after successfull authentication
            //rootFrame.Navigate(typeof(MainPage), e.Arguments);
            #endregion


            #region HOCKEYAPP SAMPLE CODE

            //Updater is only available on Windows Phone because of sideloading-restrictions on Windows 8.1
#if WINDOWS_PHONE_APP
            //initiates a call to the hockeyapp server (if network is available) and shows a dialog to the user if an update is available
            //should be called after SendCrashesAsync (if not auto-send) to avoid multiple dialogs!
            await HockeyClient.Current.CheckForAppUpdateAsync();
#endif
            #endregion

        }

        #region Code from visual studio standard template

#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif


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
#if WINDOWS_PHONE_APP
                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }


#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private async void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
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

        private async Task RestoreStatusAsync(ApplicationExecutionState previousExecutionState)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (previousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }
            }
        }

       
        /// <summary>
        /// Handle OnActivated event to deal with File Open/Save continuation activation kinds
        /// </summary>
        /// <param name="e">Application activated event arguments, it can be casted to proper sub-type based on ActivationKind</param>
        protected async override void OnActivated(IActivatedEventArgs e)
        {
            base.OnActivated(e);

            Frame rootFrame = CreateRootFrame();
            await RestoreStatusAsync(e.PreviousExecutionState);
            HockeyClient.Current.HandleReactivationOfFeedbackFilePicker(e);

            if(rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage));
            }

            Window.Current.Activate();
        }
#endif



        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
#if WINDOWS_PHONE_APP
            await SuspensionManager.SaveAsync();
#endif
            
            deferral.Complete();
        }

        #endregion

    }
}