using System;

using Mwm.BeerFactoryV2.Uwp.Burger.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Mwm.BeerFactoryV2.Uwp.Burger.Views
{
    public sealed partial class TabbedPage : Page
    {
        public TabbedViewModel ViewModel { get; } = new TabbedViewModel();

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
