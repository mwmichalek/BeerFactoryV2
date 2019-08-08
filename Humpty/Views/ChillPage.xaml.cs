using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views
{
    public sealed partial class ChillPage : Page
    {
        private ChillViewModel ViewModel => DataContext as ChillViewModel;

        public ChillPage()
        {
            InitializeComponent();
        }
    }
}
