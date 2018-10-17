using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Mwm.BeerFactoryV2.Service.Phases {
    public abstract class BeerFactoryPhase : ViewModelBase {

        private IEventAggregator _eventAggregator;

        private IBeerFactory _beerFactory;

        public BeerFactoryPhase(IEventAggregator eventAggregator, IBeerFactory beerFactory) {
            _eventAggregator = eventAggregator;
            _beerFactory = beerFactory;

            _eventAggregator.GetEvent<TemperatureChangeEvent>().Subscribe((temperatureResult) => {
                if (temperatureResult.Index == 1)
                    Temperature1 = temperatureResult.Value;
                if (temperatureResult.Index == 2)
                    Temperature2 = temperatureResult.Value;
                if (temperatureResult.Index == 3)
                    Temperature3 = temperatureResult.Value;
                if (temperatureResult.Index == 4)
                    Temperature4 = temperatureResult.Value;
                if (temperatureResult.Index == 5)
                    Temperature5 = temperatureResult.Value;
                if (temperatureResult.Index == 6)
                    Temperature6 = temperatureResult.Value;
                if (temperatureResult.Index == 7)
                    Temperature7 = temperatureResult.Value;
                if (temperatureResult.Index == 8)
                    Temperature8 = temperatureResult.Value;
                if (temperatureResult.Index == 9)
                    Temperature9 = temperatureResult.Value;
            });

            _eventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe((connectionStatus) => {
                Debug.WriteLine($"Connection Status: {connectionStatus.Type}");
                ConnectionStatus = $"{connectionStatus.Type}";
            });

            _eventAggregator.GetEvent<SsrChangeEvent>().Subscribe((ssrResult) => {
                //Debug.WriteLine($"SSR Status: {ssrResult.Index} {ssrResult.IsEngaged}");
                if (ssrResult.Index == 1) {
                    HltElementEngagedBrush = ssrResult.IsEngaged ? yellow : black;
                    HltPercentage = ssrResult.Percentage;
                } else if (ssrResult.Index == 2) {
                    BkElementEngagedBrush = ssrResult.IsEngaged ? yellow : black;
                    BkPercentage = ssrResult.Percentage;
                }
            });

            //_eventAggregator.GetEvent<KettleResultEvent>().Subscribe((kettleResult) => {
            //    Debug.WriteLine($"Kettle Shit Happened: {kettleResult.Index} {kettleResult.Percentage}");
            //    if (kettleResult.Index == 1) {
            //        //Debug.WriteLine($"HLT Percentage: {kettleResult.Percentage}");
            //        HltPercentage = kettleResult.Percentage;
            //    } else if (kettleResult.Index == 2) {
            //        //Debug.WriteLine($"BK Percentage: {kettleResult.Percentage}");
            //        BkPercentage = kettleResult.Percentage;
            //    }
            //});

            _eventAggregator.GetEvent<MessageEvent>().Subscribe((message) => {
                Debug.WriteLine($"Message: {message.Index} {message.Body}");

                if (message.Index == 1) {
                    //Debug.WriteLine($"HLT Percentage: {kettleResult.Percentage}");
                    HltPercentage = message.Percentage;
                } else if (message.Index == 2) {
                    //Debug.WriteLine($"BK Percentage: {kettleResult.Percentage}");
                    BkPercentage = message.Percentage;
                }
            });
        }

        private string connectionStatus = "";
        public string ConnectionStatus {
            get { return connectionStatus; }
            set { SetProperty(ref connectionStatus, value); }
        }

        public int hltPercentage = 0;
        public int HltPercentage {
            get { return hltPercentage; }
            set { SetProperty(ref hltPercentage, value); }
        }

        public int bkPercentage = 0;
        public int BkPercentage {
            get { return bkPercentage; }
            set { SetProperty(ref bkPercentage, value); }
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

        private decimal temperature4 = 0.0m;
        public decimal Temperature4 {
            get { return temperature4; }
            set { SetProperty(ref temperature4, value); }
        }

        private decimal temperature5 = 0.0m;
        public decimal Temperature5 {
            get { return temperature5; }
            set { SetProperty(ref temperature5, value); }
        }

        private decimal temperature6 = 0.0m;
        public decimal Temperature6 {
            get { return temperature6; }
            set { SetProperty(ref temperature6, value); }
        }

        private decimal temperature7 = 0.0m;
        public decimal Temperature7 {
            get { return temperature7; }
            set { SetProperty(ref temperature7, value); }
        }

        private decimal temperature8 = 0.0m;
        public decimal Temperature8 {
            get { return temperature8; }
            set { SetProperty(ref temperature8, value); }
        }

        private decimal temperature9 = 0.0m;
        public decimal Temperature9 {
            get { return temperature9; }
            set { SetProperty(ref temperature9, value); }
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

        //private bool hltElementEngaged = false;
        //public bool HltElementEngaged {
        //    get { return hltElementEngaged; }
        //    set { SetProperty(ref hltElementEngaged, value); }

        //}

        //private bool bkElementEngaged = false;
        //public bool BkElementEngaged {
        //    get { return bkElementEngaged; }
        //    set { SetProperty(ref bkElementEngaged, value); }

        //}

        private static SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
        private static SolidColorBrush black = new SolidColorBrush(Colors.Black);

        private Brush hltElementEngagedBrush = black;
        public Brush HltElementEngagedBrush {
            get { return hltElementEngagedBrush; }
            set { SetProperty(ref hltElementEngagedBrush, value); }
        }

        private Brush bkElementEngagedBrush = black;
        public Brush BkElementEngagedBrush {
            get { return bkElementEngagedBrush; }
            set { SetProperty(ref bkElementEngagedBrush, value); }
        }

        public void HltPublishChangeEvent() {
            //_eventAggregator.GetEvent<KettleCommandEvent>().Publish(new KettleCommand { Index = 1, Percentage = hltPercentageSetting });
        }

        public void BkPublishChangeEvent() {
            //_eventAggregator.GetEvent<KettleCommandEvent>().Publish(new KettleCommand { Index = 2, Percentage = bkPercentageSetting });
        }
    }
}
