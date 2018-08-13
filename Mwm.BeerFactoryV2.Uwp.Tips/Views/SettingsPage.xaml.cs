﻿using System;

using Mwm.BeerFactoryV2.Uwp.Tips.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Mwm.BeerFactoryV2.Uwp.Tips.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel => DataContext as SettingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
