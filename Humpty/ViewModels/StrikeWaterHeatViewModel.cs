using System;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Events;
using Mwm.BeerFactoryV2.Service.Pid;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;
using Serilog;

namespace Humpty.ViewModels {
    public class StrikeWaterHeatViewModel : DisplayEventHandlerViewModelBase {

        private ILogger Logger { get; set; }

        public StrikeWaterHeatViewModel(IBeerFactory beerFactory, IEventAggregator eventAggregator) : base(eventAggregator) {
            Logger = Log.Logger;
            //MyAwesomeCommand = new DelegateCommand<string>(ExecuteMyAwesomeCommand, (str) => Test == "Balls").ObservesProperty(() => Test);

            UpdatePidEnabledCommand = new DelegateCommand(UpdatePid);
            //UpdatePidSetPointCommand = new DelegateCommand(UpdatePid);

            var hltThermometer = beerFactory.Thermometers.GetById(ThermometerId.HLT);
            if (hltThermometer != null)
                HltTemperature = (double)hltThermometer.Temperature;
        }

        public StrikeWaterHeatViewModel() {
        }

        public StrikeWaterHeatViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {
        }

        //public DelegateCommand<string> MyAwesomeCommand { get; private set; }

        private bool _isEnabled = true;

        //public bool IsEnabled {
        //    get { return Test == "Balls"; }
        //}

        public override void TemperatureChangeOccured(TemperatureChange temperatureChange) {

            if (temperatureChange.Id == ThermometerId.HLT) {
                //Logger.Information($"HLT Change");
                HltTemperature = Math.Round((double)temperatureChange.Value, 1);
            }
        }

        public override void ConnectionStatusOccured(ConnectionStatus connectionStatus) {
            //Title = $"ConnectionStatus: {connectionStatus.Status.ToString()}";
        }


        public DelegateCommand UpdatePidEnabledCommand { get; private set; }
        

        public void UpdatePid() {
            PidRequestFire(new PidRequest {
                Id = PidControllerId.HLT,
                IsEngaged = Engaged,
                SetPoint = _hltSetpoint,
                PidMode = PidMode.Temperature
            });

        }

        //private void ExecuteMyAwesomeCommand(string balls) {

        //}

        //private string _title = "Balls";

        //public string Title {
        //    get { return _title; }
        //    set { SetProperty(ref _title, value); }
        //}

        //private string _test = "Balls";

        //public string Test {
        //    get { return _test; }
        //    set {
        //        SetProperty(ref _test, value);
        //    }
        //}

        //private void TempTest(ThermometerChange tc) {
        //    if (tc.Id == ThermometerId.HLT)
        //        Title = tc.Value.ToString();
        //}


        private bool _engaged;

        public bool Engaged {
            get { return _engaged; }
            set {
                SetProperty(ref _engaged, value);
                //TODO: Switch to delegate
                //UpdatePid();
            }
        }
               
        private double _hltTemperature;

        public double HltTemperature {
            get { return _hltTemperature; }
            set {
                SetProperty(ref _hltTemperature, value);
                //Logger.Information($"HLT: {_hltTemperature}");
            }
        }

        public DelegateCommand UpdatePidSetPointCommand { get; private set; }

        private int _hltSetpoint;

        public int HltSetpoint {
            get { return _hltSetpoint; }
            set {
                SetProperty(ref _hltSetpoint, value);
                //TODO: Switch to delegate
                //UpdatePid();
            }
        }

    }
}
