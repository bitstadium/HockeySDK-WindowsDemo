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

        private ILogger logger = LogManagerFactory.DefaultLogManager.GetLogger<MainPage>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AuthorizeRedirectButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.AuthorizeUser(typeof(Authorized), eMail: DemoConstants.USER_EMAIL);
        }

        private void IdentifyRedirectButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.IdentifyUser(DemoConstants.YOUR_APP_SECRET, typeof(Authorized));
        }

        private void AuthorizeActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Texblock1.Text = "";
            HockeyClient.Current.AuthorizeUser(async () => { this.Texblock1.Text = "Authorized"; await new MessageDialog("fff").ShowAsync(); }, eMail: DemoConstants.USER_EMAIL);
        }

        private void ExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            logger.Warn("Some logged information.");
            throw new Exception("TestException from DemoApp");
        }

        private void AggregateExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            var aggr = new AggregateException("AggregateExceptionFromDemoApp", new ArgumentException("TestArgumentException from DemoApp"), new InvalidOperationException("InvalidOperationException from DemoApp"));
            throw aggr;
        }

        private void BackgroundExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            ThrowUncaughtBackgroundException();
            new MessageDialog("Uncaught background exception has been thrown.").ShowAsync();
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

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await HockeyClient.Current.LogoutUserAsync();
            HockeyClient.Current.LogoutFromFeedback();
        }
    }
}
