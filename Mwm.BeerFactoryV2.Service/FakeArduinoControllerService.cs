using CommandMessenger;
using CommandMessenger.Transport.Serial;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Mwm.BeerFactoryV2.Service {
    public class FakeArduinoControllerService : ArduinoControllerService {
        enum Command {
            Acknowledge,
            Error,
            PingRequest,
            PingResult,
            StatusRequest,
            StatusResult,
            KettleRequest,
            KettleResult,
            TempChange,
            SsrChange,
            HeaterChange,
            PumpChange
        };

        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;
        public bool IsConnected { get; set; }

        private IEventAggregator _eventAggregator;

        public FakeArduinoControllerService(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
        }

        public async void Run() {
            while (true) {
                var setupResult = await Setup();
                if (setupResult) {
                    while (IsConnected) {
                        Ping();
                        Thread.Sleep(1000);
                    }
                    Exit();

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.Disconnected });
                    });
                } else {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.NotConnected });
                    });
                }
                Thread.Sleep(1000);
            }
        }


        public async Task<bool> Setup() {

            IsConnected = true;


            if (IsConnected) {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.Connected });
                });
                
                RequestStatus();
            }

            return IsConnected;
        }

        public void Ping() {
            IsConnected = true;
        }

        public void Exit() {

        }

        public void RequestStatus() {

        }

    }
}

