using System;

using Humpty.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Humpty.Views {
    public sealed partial class StrikeWaterHeatPage : Page {
        private StrikeWaterHeatViewModel ViewModel => DataContext as StrikeWaterHeatViewModel;

        public StrikeWaterHeatPage() {
            InitializeComponent();

            //HtlSetpointSlider.
        }

        private void ToggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            ViewModel.UpdatePid();
        }


    }
}
