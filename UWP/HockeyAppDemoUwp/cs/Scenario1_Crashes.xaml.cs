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
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Microsoft.HockeyApp;
using System.Threading.Tasks;
using System.Globalization;

namespace HockeyAppDemo
{
    public sealed partial class Scenario1_Crashes : Page
    {
        public object Thread { get; private set; }

        public Scenario1_Crashes()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This is the click handler for the 'Display' button.
        /// </summary>
        private void ExceptionButton_Click()
        {
            throw new Exception("TestException from DemoApp");
        }

        private void TrackEventButton_Click()
        {
            HockeyClient.Current.TrackEvent("Button Clicked");
        }

        private void TrackEventWithDimensionsButton_Click()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Property1", "Value 1");
            properties.Add("Property2", "Value 2");

            Dictionary<string, double> metrics = new Dictionary<string, double>();
            metrics.Add("Metric1", 1);
            metrics.Add("Metric2", 2);
            HockeyClient.Current.TrackEvent("Button Clicked with Dimensions", properties, metrics);
        }

        private void TrackTraceButton_Click()
        {
            HockeyClient.Current.TrackTrace("TrackTraceButton_Click method started successfully.");
        }

        private void TrackTraceWithSeverityLevelButton_Click()
        {
            HockeyClient.Current.TrackTrace("TrackTraceButton_Click method finished successfully.", SeverityLevel.Information);
        }

        private void TrackTraceWithPropertiesButton_Click()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Property3", "Value 3");
            properties.Add("Property4", "Value 4");

            HockeyClient.Current.TrackTrace("TrackTraceButton_Click method finished successfully.", properties);
        }

        private void TrackMetricButton_Click()
        {
            HockeyClient.Current.TrackMetric("TrackMetricButton_Click_Count", 1);
        }

        private void TrackPageViewButton_Click()
        {
            HockeyClient.Current.TrackPageView("Scenario1_Crashes");
        }

        private void TrackExceptionButton_Click()
        {
            HockeyClient.Current.TrackException(new ArgumentException("This is test handled exception!"));
        }

        private void TrackUnobservedTaskException()
        {
            Task.Factory.StartNew(() =>
            {
                throw new ArgumentNullException("This exception is UnobserveredTaskException");
            });

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void TrackDependencyButton_Click()
        {
            HockeyClient.Current.TrackDependency("https://github.com/bitstadium/HockeySDK-WindowsDemo", "HTTP", DateTime.UtcNow, TimeSpan.FromMilliseconds(100), true);
        }

        private void TrackExceptionWithSpanishLanguage_Click()
        {
            MethodA();
        }

        private void MethodA()
        {
            MethodB();
        }

        private void MethodB()
        {
            MethodC();
        }

        private void MethodC()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            CultureInfo.CurrentCulture = new CultureInfo("es-ES");
            throw new ArgumentException("Hello from Spain!");
        }
    }
}
