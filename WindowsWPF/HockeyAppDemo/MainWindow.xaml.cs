namespace HockeyAppDemoWPF
{
    using Microsoft.HockeyApp;
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

        private async void UpdateCheckButton_Click(object sender, RoutedEventArgs e)
        {
            var newVersionAvailable = await HockeyClient.Current.CheckForUpdatesAsync(true, () =>
            {
                if (Application.Current.MainWindow != null) { Application.Current.MainWindow.Close(); }
                return true;
            });
            if (!newVersionAvailable)
            {
                MessageBox.Show("We could not find a new version!");
            }
        }

        private void BackgroundExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            ThrowUncaughtBackgroundException();
            MessageBox.Show("Uncaught background exception has been thrown.");
        }

        private async void ThrowUncaughtBackgroundException()
        {
            var task = Task<bool>.Run(() => { throw new InvalidOperationException("BackgroundException"); return false; });
            //var x = await task;
        }

        private void HandleHandledException_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new InvalidOperationException("Something bad happened here");
            }
            catch (Exception ex)
            {
                (HockeyClient.Current as HockeyClient).HandleException(ex);
                // Environment.Exit(-1);
            }
        }
    }
}
