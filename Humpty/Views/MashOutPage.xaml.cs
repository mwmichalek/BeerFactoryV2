using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views
{
    public sealed partial class MashOutPage : Page
    {
        private MashOutViewModel ViewModel => DataContext as MashOutViewModel;

        public MashOutPage()
        {
            InitializeComponent();
        }
    }
}
