using System;
using System.Threading;
using System.Threading.Tasks;
using Prism.Windows.Mvvm;


namespace Mwm.BeerFactoryV2.Uwp.Tips.ViewModels {
    public class MainViewModel : ViewModelBase {
        public MainViewModel() {
            //Task.Run(() => {
            //    IncrementTemp();
            //});

        }


        private decimal temperature1 = 0.0m;
        public decimal Temperature1 {
            get { return temperature1; }
            set {
                temperature1 = value;
                RaisePropertyChanged("Temperature1");
            }
        }

        private void IncrementTemp() {
            while (true) {
                //Windows.ApplicationModel.Core.CoreApplication.Views.MainView.CoreWindow.Dispatcher.RunAsync


                Temperature1 += 0.1m;
                Thread.Sleep(1000);
            }

        }
    }
}
