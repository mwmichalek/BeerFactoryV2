using System;

using PoopSkooter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PoopSkooter.Views
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
