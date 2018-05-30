using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Dto;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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
            System.Console.WriteLine($"TemperatureResult: Index[{tempertureResult.Index}] Value[{tempertureResult.Value}]");
        }

        public void HandleConnectionStatusEvent(object sender, ConnectionStatusEvent connectionStatusEvent) {
            System.Console.WriteLine($"ConnectionStatus: {connectionStatusEvent.Type}");
        }

        public void HandleSsrResultEvent(object sender, SsrResult ssrResult) {
            System.Console.WriteLine($"SsrResult: Index[{ssrResult.Index}] IsEnaged[{ssrResult.IsEngaged}]");
        }

        public void HandleHeaterResultEvent(object sender, HeaterResult heaterResult) {
            System.Console.WriteLine($"HeaterResult: Index[{heaterResult.Index}] IsEnaged[{heaterResult.IsEngaged}]");
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
