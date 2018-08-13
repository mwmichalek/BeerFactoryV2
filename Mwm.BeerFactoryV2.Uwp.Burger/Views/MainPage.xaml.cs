using System;

using Mwm.BeerFactoryV2.Uwp.Burger.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Mwm.BeerFactoryV2.Uwp.Burger.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
