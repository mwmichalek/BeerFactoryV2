using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Dto;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Core;

namespace Mwm.BeerFactoryV2.Uwp.Cinch.Views {
    public sealed partial class MainPage : Page, INotifyPropertyChanged {
        public MainPage() {
            InitializeComponent();

            var controller = ArduinoController.Current;

            controller.ConnectionStatusEventHandler += HandleConnectionStatusEvent;
            controller.TemperatureResultEventHandler += HandleTemperatureResultEvent;
            controller.SsrResultEventHandler += HandleSsrResultEvent;
            controller.HeaterResultEventHandler += HandleHeaterResultEvent;

            Task.Run(() => controller.Run());

        }

        public void HandleTemperatureResultEvent(object sender, TemperatureResult tempertureResult) {
            Debug.WriteLine($"TemperatureResult: Index[{tempertureResult.Index}] Value[{tempertureResult.Value}]");

            switch (tempertureResult.Index) {
                case 1:
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        TemperatureValueText1.Text = $"{tempertureResult.Value}";
                    });
                    break;

                case 2:
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        TemperatureValueText2.Text = $"{tempertureResult.Value}";
                    });
                    break;

                case 3:
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        TemperatureValueText3.Text = $"{tempertureResult.Value}";
                    });
                    break;
            }
 
        }

        public void HandleConnectionStatusEvent(object sender, ConnectionStatusEvent connectionStatusEvent) {
            Debug.WriteLine($"ConnectionStatus: {connectionStatusEvent.Type}");
        }

        public void HandleSsrResultEvent(object sender, SsrResult ssrResult) {
            Debug.WriteLine($"SsrResult: Index[{ssrResult.Index}] IsEnaged[{ssrResult.IsEngaged}]");

            switch (ssrResult.Index) {
                case 5:
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        SsrValueText1.Text = $"{ssrResult.IsEngaged}";
                    });
                    break;

                case 6:
                    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        SsrValueText2.Text = $"{ssrResult.IsEngaged}";
                    });
                    break;
            }
        }

        public void HandleHeaterResultEvent(object sender, HeaterResult heaterResult) {
            Debug.WriteLine($"HeaterResult: Index[{heaterResult.Index}] IsEnaged[{heaterResult.IsEngaged}]");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null) {
            if (Equals(storage, value)) {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
