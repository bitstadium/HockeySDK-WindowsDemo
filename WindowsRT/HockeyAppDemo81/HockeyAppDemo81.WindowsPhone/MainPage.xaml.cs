using Microsoft.HockeyApp;
using MetroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Management.Deployment;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace HockeyAppDemo81
{
    public sealed partial class MainPage : Page
    {

        #region HOCKEYAPP SAMPLE CODE

        private ILogger logger = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();

        private void AuthorizeRedirectButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.AuthorizeUser(typeof(Authorized), eMail: DemoConstants.USER_EMAIL);
        }

        private void IdentifyRedirectButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.IdentifyUser(DemoConstants.YOUR_APP_SECRET, typeof(Identified));
        }

        private void ExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            logger.Error("Some imporant thing to log");
            throw new Exception("TestException from DemoApp");
        }

        private void AggregateExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            var aggr = new AggregateException("AggregateExceptionFromDemoApp", new ArgumentException("TestArgumentException from DemoApp"), new InvalidOperationException("InvalidOperationException from DemoApp"));
            throw aggr;
        }

        private async void BackgroundExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            ThrowUncaughtBackgroundException();
            await new MessageDialog("Uncaught background exception has been thrown.").ShowAsync();
        }

        private async void ThrowUncaughtBackgroundException()
        {
            var task = Task<bool>.Run(() => { throw new InvalidOperationException("BackgroundException"); return false; });
            //var x = await task;
        }

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.ShowFeedback(DemoConstants.USER_NAME, DemoConstants.USER_EMAIL);
        }

        private async void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {

            await HockeyClient.Current.CheckForAppUpdateAsync(new UpdateCheckSettings()
            {
                UpdateMode = UpdateMode.InApp
            });
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await HockeyClient.Current.LogoutUserAsync();
            await HockeyClient.Current.LogoutFromFeedbackAsync();
        }

        #endregion

        #region Standard template code

        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();

            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;    

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Prepare page for display here.

            base.OnNavigatedTo(e);
            

            // If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        #endregion

        private void TrackEvent_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.TrackEvent("Button clicked");
        }
    }
}
