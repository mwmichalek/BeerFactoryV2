using System;

using PoopSkooter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PoopSkooter.Views
{
    public sealed partial class MashPage : Page
    {
        private MashViewModel ViewModel => DataContext as MashViewModel;

        public MashPage()
        {
            InitializeComponent();
        }
    }
}
