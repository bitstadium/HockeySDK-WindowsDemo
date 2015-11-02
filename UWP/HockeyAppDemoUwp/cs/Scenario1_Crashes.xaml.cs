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
using Windows.UI.Xaml.Controls;

namespace HockeyAppDemo
{
    public sealed partial class Scenario1_Crashes : Page
    {
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
    }
}
