using System;

using BareBallz.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BareBallz.Views
{
    public sealed partial class StrikePage : Page
    {
        private StrikeViewModel ViewModel => DataContext as StrikeViewModel;

        public StrikePage()
        {
            InitializeComponent();
        }
    }
}
