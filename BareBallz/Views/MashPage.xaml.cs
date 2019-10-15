using System;

using BareBallz.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BareBallz.Views
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
