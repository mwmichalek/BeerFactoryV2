using System;

using Mwm.BeerFactoryV2.Uwp.Tips.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mwm.BeerFactoryV2.Uwp.Tips.Views {
    public sealed partial class MainPage : Page {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage() {
            InitializeComponent();
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

    }
}
