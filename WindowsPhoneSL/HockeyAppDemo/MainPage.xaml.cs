using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HockeyAppDemo.Resources;
using HockeyApp;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace HockeyAppDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings["myKey"] = "Some description of an exception.";
        }

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.ShowFeedback();
        }

        private void IdentifyButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.IdentifyUser(DemoConstants.YOUR_APP_SECRET, new Uri("/Identified.xaml", UriKind.Relative));
        }

        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.AuthorizeUser(new Uri("/Authorized.xaml", UriKind.Relative));
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.LogoutUser();
            MessageBox.Show("You have been logged out.");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            HockeyClient.Current.CheckForUpdates(new UpdateCheckSettings { UpdateMode = UpdateMode.InApp, EnforceUpdateIfMandatory= true });
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
            MessageBox.Show("Uncaught background exception has been thrown.");
            GC.Collect();
        }

        private async void ThrowUncaughtBackgroundException()
        {
            var task = Task<bool>.Run(() => { throw new InvalidOperationException("BackgroundException"); return false; });
            var x = await task;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}