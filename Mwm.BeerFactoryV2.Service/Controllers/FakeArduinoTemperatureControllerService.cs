using CommandMessenger;
using CommandMessenger.Transport.Serial;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Usb;
using Windows.Storage.Streams;
using Windows.UI.Core;
using SerialPortLib;
using SerialArduino;
using System.Linq;
using Mwm.BeerFactoryV2.Service.Components;

namespace Mwm.BeerFactoryV2.Service.Controllers {
    public class FakeArduinoTemperatureControllerService : TemperatureControllerService {

        private IBeerFactory _beerFactory;

        private IEventManager _eventManager;

        public FakeArduinoTemperatureControllerService(IBeerFactory beerFactory, IEventManager eventManager) {
            _beerFactory = beerFactory;
            _eventManager = eventManager;
        }

        private List<decimal> temperatures = new List<decimal> { 70.01m, 69.54m, 70.12m,
                                                                 70.43m, 69.72m, 68.91m,
                                                                 71.44m, 70.54m, 69.87m };

        private Random rnd = new Random();

        public override async Task Run() {

            foreach (var temperature in temperatures.Select((value, index) => new { Value = value, Index = index + 1 })) {
                //await _beerFactory.UpdateTemperatureAsync((ThermometerId)temperature.Index, temperature.Value);

                //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                //    _eventAggregator.GetEvent<TemperatureChangeEvent>().Publish(new TemperatureChange { Index = temperature.Index, Value = temperature.Value });
                //});

                //_eventManager.Publish<ThermometerChangeEvent>(new ThermometerChange { Index = temperature.Index, Value = temperature.Value });

            }

            while (true) {
                try {
                    int index = rnd.Next(0, 10);
                    temperatures[index] += rnd.NextDecimal();

                    //await _beerFactory.UpdateTemperatureAsync((ThermometerId)(index + 1), temperatures[index]);

                    //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    //    _eventAggregator.GetEvent<TemperatureChangeEvent>().Publish(new TemperatureChange { Index = index + 1, Value = temperatures[index] });
                    //});

                    //_eventManager.Publish<TemperatureChange>(new TemperatureChange { Index = index + 1, Value = temperatures[index] });
                } catch (Exception ex) {

                }

                Thread.Sleep(1000);
            }

        }

    }

    public static class Bullshit {

        public static decimal NextDecimal(this Random rnd) {
            decimal tempChange = (rnd.Next(0, 20) / 100) - 0.10m;
            return tempChange;
        }
    }

}

