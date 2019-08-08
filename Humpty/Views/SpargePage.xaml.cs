using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views
{
    public sealed partial class SpargePage : Page
    {
        private SpargeViewModel ViewModel => DataContext as SpargeViewModel;

        public SpargePage()
        {
            InitializeComponent();
        }
    }
}
