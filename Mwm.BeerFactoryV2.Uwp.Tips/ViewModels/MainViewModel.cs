using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using Prism.Windows.Mvvm;


namespace Mwm.BeerFactoryV2.Uwp.Tips.ViewModels {
    public class MainViewModel : ViewModelBase {

        private IEventAggregator _eventAggregator;

        public MainViewModel(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<TemperatureResultEvent>().Subscribe((temperatureResult) => {
                if (temperatureResult.Index == 1)
                    Temperature1 = temperatureResult.Value;
                if (temperatureResult.Index == 2)
                    Temperature2 = temperatureResult.Value;
                if (temperatureResult.Index == 3)
                    Temperature3 = temperatureResult.Value;
            });

            _eventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe((connectionStatus) => {
                Debug.WriteLine($"Connection Status: {connectionStatus.Type}");
                ConnectionStatus = $"{connectionStatus.Type}";
            });
        }

        public string connectionStatus = "";
        public string ConnectionStatus {
            get { return connectionStatus; }
            set { SetProperty(ref connectionStatus, value); }
        }

        private decimal temperature1 = 0.0m;
        public decimal Temperature1 {
            get { return temperature1; }
            set { SetProperty(ref temperature1, value); }
        }

        private decimal temperature2 = 0.0m;
        public decimal Temperature2 {
            get { return temperature2; }
            set { SetProperty(ref temperature2, value); }
        }

        private decimal temperature3 = 0.0m;
        public decimal Temperature3 {
            get { return temperature3; }
            set { SetProperty(ref temperature3, value); }
        }

        private int hltPercentageSetting = 0;
        public int HltPercentageSetting {
            get { return hltPercentageSetting; }
            set {
                SetProperty(ref hltPercentageSetting, value);
                //Debug.WriteLine($"HltPercentageSetting: {value}");
                //_eventAggregator.GetEvent<KettleCommandEvent>().Publish(new KettleCommand { Index = 1, Percentage = value });
            }
        }

        private int bkPercentageSetting = 0;
        public int BkPercentageSetting {
            get { return bkPercentageSetting; }
            set {
                SetProperty(ref bkPercentageSetting, value);
                //Debug.WriteLine($"BkPercentageSetting: {value}");
                //_eventAggregator.GetEvent<KettleCommandEvent>().Publish(new KettleCommand { Index = 2, Percentage = value });
            }
        }

        public void HltPublishChangeEvent() {
            _eventAggregator.GetEvent<KettleCommandEvent>().Publish(new KettleCommand { Index = 1, Percentage = hltPercentageSetting });
        }

        public void BkPublishChangeEvent() {
            _eventAggregator.GetEvent<KettleCommandEvent>().Publish(new KettleCommand { Index = 2, Percentage = bkPercentageSetting });
        }

    }
}
