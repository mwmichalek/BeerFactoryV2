using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views
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
