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

        private void StrikeWaterSetPointSlider_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            ViewModel.StrikeWaterSetPointPublishChangeEvent();
        }

        private void MashSetPointSlider_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            ViewModel.MashSetPointPublishChangeEvent();
        }

        private void BoilKettleSetPointSlider_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            ViewModel.BoilKettleSetPointPublishChangeEvent();
        }
    }
}
