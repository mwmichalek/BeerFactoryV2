using System;

using Skooter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Skooter.Views
{
    public sealed partial class ContentGridPage : Page
    {
        private ContentGridViewModel ViewModel => DataContext as ContentGridViewModel;

        public ContentGridPage()
        {
            InitializeComponent();
        }
    }
}
