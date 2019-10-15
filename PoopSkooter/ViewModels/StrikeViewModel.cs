using System;
using Microsoft.AspNetCore.SignalR.Client;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Events;
using Mwm.BeerFactoryV2.Service.Pid;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;
using Serilog;

namespace PoopSkooter.ViewModels {
    public class StrikeViewModel : DisplayEventHandlerViewModelBase {
        private ILogger Logger { get; set; }

        private HubConnection connection;

        public StrikeViewModel(IBeerFactory beerFactory, IEventAggregator eventAggregator) : base(eventAggregator) {
            Logger = Log.Logger;
            //MyAwesomeCommand = new DelegateCommand<string>(ExecuteMyAwesomeCommand, (str) => Test == "Balls").ObservesProperty(() => Test);

            UpdatePidEnabledCommand = new DelegateCommand(UpdatePid);
            //UpdatePidSetPointCommand = new DelegateCommand(UpdatePid);

            var hltThermometer = beerFactory.Thermometers.GetById(ThermometerId.HLT);
            if (hltThermometer != null)
                HltTemperature = (double)hltThermometer.Temperature;


            var hltPidController = beerFactory.PidControllers.GetById(PidControllerId.HLT);
            if (hltPidController != null)
                HltSetpoint = (int)hltPidController.SetPoint;


            connection = new HubConnectionBuilder()
                .WithUrl("https://emrsd-ws-bf.azurewebsites.net/ChatHub")
                .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) => {
                Logger.Information($"User: {user}, Message: {message}");
            });

            try {
                connection.StartAsync();
                
            } catch (Exception ex) {
           
            }

        }

        public StrikeViewModel() {

        }

        public StrikeViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {
        }


        //public DelegateCommand<string> MyAwesomeCommand { get; private set; }

        public override void TemperatureChangeOccured(TemperatureChange temperatureChange) {

            if (temperatureChange.Id == ThermometerId.HLT) {
                //Logger.Information($"HLT Change");
                HltTemperature = Math.Round((double)temperatureChange.Value, 1);
                connection.InvokeAsync("SendMessage",
                    "Temp Change", temperatureChange.Value.ToString());
            }
        }

        public override void SsrChangeOccured(SsrChange ssrChange) {
            if (ssrChange.Id == SsrId.HLT) {
                HltSsrPercentage = ssrChange.Percentage;
            }
            base.SsrChangeOccured(ssrChange);
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
                HltTemperatureStr = _hltTemperature.ToString("0.0");
                //Logger.Information($"HLT: {_hltTemperature}");
            }
        }

        private string _hltTemperatureStr;

        public string HltTemperatureStr {
            get { return _hltTemperatureStr; }
            set {
                SetProperty(ref _hltTemperatureStr, value);
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

        private int _hltSsrPercentage;

        public int HltSsrPercentage {
            get { return _hltSsrPercentage; }
            set {
                SetProperty(ref _hltSsrPercentage, value);
                //TODO: Switch to delegate
                //UpdatePid();
            }
        }

    }
}
