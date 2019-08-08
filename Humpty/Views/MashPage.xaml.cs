using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views
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
