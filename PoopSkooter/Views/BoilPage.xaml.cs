using System;

using PoopSkooter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PoopSkooter.Views
{
    public sealed partial class BoilPage : Page
    {
        private BoilViewModel ViewModel => DataContext as BoilViewModel;

        public BoilPage()
        {
            InitializeComponent();
        }
    }
}
