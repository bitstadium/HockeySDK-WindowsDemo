using HockeyApp;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyAppDemo81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            HockeyClient.Current.IdentifyUser("ae93934ac5664cfd6745d4ae997d0fe8", typeof(Authorized));
        }

        private void AuthorizeActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Texblock1.Text = "";
            HockeyClient.Current.AuthorizeUser(async () => { this.Texblock1.Text = "Authorized"; await new MessageDialog("fff").ShowAsync(); }, eMail: DemoConstants.USER_EMAIL);
        }

        private void ExceptionButton_Click(object sender, RoutedEventArgs e)
        {
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
            GC.Collect();
        }

        private async void ThrowUncaughtBackgroundException()
        {
            var task = Task<bool>.Run(() => { throw new InvalidOperationException("BackgroundException"); return false; });
            var x = await task;
        }

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.ShowFeedback(DemoConstants.USER_NAME, DemoConstants.USER_EMAIL);
        }

    }
}
