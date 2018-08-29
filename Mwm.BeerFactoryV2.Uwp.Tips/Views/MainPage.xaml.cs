using System;
using System.Diagnostics;
using Mwm.BeerFactoryV2.Uwp.Tips.ViewModels;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mwm.BeerFactoryV2.Uwp.Tips.Views {
    public sealed partial class MainPage : SessionStateAwarePage {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage() {
            InitializeComponent();
        }

        private void HltSlider_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            ViewModel.HltPublishChangeEvent();
        }

        private void BkSlider_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            ViewModel.BkPublishChangeEvent();
        }
    }
}
