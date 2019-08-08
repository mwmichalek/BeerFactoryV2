using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views
{
    public sealed partial class StrikeWaterTransferPage : Page
    {
        private StrikeWaterTransferViewModel ViewModel => DataContext as StrikeWaterTransferViewModel;

        public StrikeWaterTransferPage()
        {
            InitializeComponent();
        }
    }
}
