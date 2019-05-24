using System;

using Skooter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Skooter.Views
{
    public sealed partial class TabbedPage : Page
    {
        private TabbedViewModel ViewModel => DataContext as TabbedViewModel;

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
