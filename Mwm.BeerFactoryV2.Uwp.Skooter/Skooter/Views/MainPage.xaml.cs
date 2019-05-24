using System;

using Skooter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Skooter.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
