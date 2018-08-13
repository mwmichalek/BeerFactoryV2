using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Mwm.BeerFactoryV2.Uwp.Cinch.Services {
    public class BeerFactoryService {

        private static BeerFactoryService _beerFactoryServiceInstance;

        private ArduinoController _arduinoController = ArduinoController.Current;

        public BeerFactoryModel BeerFactoryModel { get; } = new BeerFactoryModel();


        private BeerFactoryService() {
            _arduinoController.ConnectionStatusEventHandler += HandleConnectionStatusEvent;
            _arduinoController.TemperatureResultEventHandler += HandleTemperatureResultEvent;
            _arduinoController.SsrResultEventHandler += HandleSsrResultEvent;
            _arduinoController.HeaterResultEventHandler += HandleHeaterResultEvent;

            Task.Run(() => _arduinoController.Run());
        }

        public static BeerFactoryService Current {
            get {
                if (_beerFactoryServiceInstance == null) {
                    _beerFactoryServiceInstance = new BeerFactoryService();
                }
                return _beerFactoryServiceInstance;
            }
        }

        public void HandleTemperatureResultEvent(object sender, TemperatureResult tempertureResult) {
            Debug.WriteLine($"TemperatureResult: Index[{tempertureResult.Index}] Value[{tempertureResult.Value}]");

            switch (tempertureResult.Index) {
                case 1:
                    BeerFactoryModel.Temperature1 = tempertureResult.Value;
                    break;

                case 2:
                    BeerFactoryModel.Temperature2 = tempertureResult.Value;
                    break;

                case 3:
                    BeerFactoryModel.Temperature3 = tempertureResult.Value;
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
                    BeerFactoryModel.SsrEnagaged1 = ssrResult.IsEngaged;
                    break;

                case 6:
                    BeerFactoryModel.SsrEnagaged2 = ssrResult.IsEngaged;
                    break;
            }
        }

        public void HandleHeaterResultEvent(object sender, HeaterResult heaterResult) {
            Debug.WriteLine($"HeaterResult: Index[{heaterResult.Index}] IsEnaged[{heaterResult.IsEngaged}]");
        }

        
    }

    public class BeerFactoryModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;


        private decimal _temperature1 = 0.0m;
        public decimal Temperature1 {
            get {
                return _temperature1;
            }
            set {
                _temperature1 = value;
                OnPropertyChanged("Temperature1");
            }
        }

        private decimal _temperature2 = 0.0m;
        public decimal Temperature2 {
            get {
                return _temperature2;
            }
            set {
                _temperature2 = value;
                OnPropertyChanged("Temperature2");
            }
        }

        private decimal _temperature3 = 0.0m;
        public decimal Temperature3 {
            get {
                return _temperature3;
            }
            set {
                _temperature3 = value;
                OnPropertyChanged("Temperature3");
            }
        }

        public bool SsrEnagaged1 { get; set; } = false;

        public bool SsrEnagaged2 { get; set; } = false;

        private void OnPropertyChanged(string propertyName) {
            CoreApplication.GetCurrentView().CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }).GetResults();
        }
    }
}
