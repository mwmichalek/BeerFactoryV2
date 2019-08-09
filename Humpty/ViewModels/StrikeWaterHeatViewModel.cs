using System;
using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace Humpty.ViewModels {
    public class StrikeWaterHeatViewModel : DisplayEventHandlerViewModelBase {

        public StrikeWaterHeatViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {
            MyAwesomeCommand = new DelegateCommand<string>(ExecuteMyAwesomeCommand, (str) => Test == "Balls").ObservesProperty(() => Test);
        }

        public DelegateCommand<string> MyAwesomeCommand { get; private set; }

        private bool _isEnabled = true;

        public bool IsEnabled {
            get { return Test == "Balls"; }
        }

        public override void TemperatureChangeOccured(TemperatureChange temperatureChange) {
            if (temperatureChange.Id == ThermometerId.HLT)
                Title = temperatureChange.Value.ToString();
        }

        public override void ConnectionStatusOccured(ConnectionStatus connectionStatus) {
            Title = $"ConnectionStatus: {connectionStatus.Status.ToString()}";
        }

        private void ExecuteMyAwesomeCommand(string balls) {

        }

        private string _title = "Balls";

        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _test = "Balls";

        public string Test {
            get { return _test; }
            set {
                SetProperty(ref _test, value);
            }
        }

        private void TempTest(ThermometerChange tc) {
            if (tc.Id == ThermometerId.HLT)
                Title = tc.Value.ToString();
        }

        private int _hltSetpoint;

        public int HltSetpoint {
            get { return _hltSetpoint; }
            set {
                SetProperty(ref _hltSetpoint, value);
                Title = value.ToString();
            }
        }

        private int _mtSetpoint;

        public int MtSetpoint {
            get { return _mtSetpoint; }
            set {
                SetProperty(ref _mtSetpoint, value);
            }
        }
    }
}
