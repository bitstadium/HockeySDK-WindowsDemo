namespace BackgroundServices
{
    using System;
    using System.Linq;
    using Windows.ApplicationModel.Background;

    public sealed class MyBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // just the demoe exception that shows that Background Task excpeptions are not propagated to global exception handler on
            // CoreApplication.UnhandledErrorDetected and therefore it is not monitored by HockeyApp.
            
            // To invoke Background Task, just use LifeCycle Events dropdown in Visual Studio 2015.
            throw new NotImplementedException();
        }

        public static async void Register()
        {
            var isRegistered = BackgroundTaskRegistration.AllTasks.Values.Any(
                t => t.Name == nameof(MyBackgroundTask));

            if (isRegistered)
                return;

            if (await BackgroundExecutionManager.RequestAccessAsync() == BackgroundAccessStatus.Denied)
                return;

            var builder = new BackgroundTaskBuilder
            {
                Name = nameof(MyBackgroundTask),
                TaskEntryPoint = $"{nameof(BackgroundServices)}.{nameof(MyBackgroundTask)}"
            };

            builder.SetTrigger(new TimeTrigger(120, false));
            builder.Register();
        }
    }
}
