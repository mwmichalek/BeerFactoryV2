﻿using CommandMessenger;
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

namespace Mwm.BeerFactoryV2.Service.Controllers {
    public class FakeArduinoTemperatureControllerService : TemperatureControllerService {

        public FakeArduinoTemperatureControllerService(IBeerFactory beerFactory) {

        }


        private IEventAggregator _eventAggregator;

        private List<decimal> temperatures = new List<decimal> { 70.01m, 69.54m, 70.12m,
                                                                 70.43m, 69.72m, 68.91m,
                                                                 71.44m, 70.54m, 69.87m };

        private Random rnd = new Random();

        public FakeArduinoTemperatureControllerService(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            //Random rnd = new Random();
            //int month = rnd.Next(1, 13); // creates a number between 1 and 12
        }

        public override async Task Run() {

            foreach (var temperature in temperatures.Select((value, index) => new { Value = value, Index = index })) {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _eventAggregator.GetEvent<TemperatureChangeEvent>().Publish(new TemperatureChange { Index = temperature.Index, Value = temperature.Value });
                });
            }

            while (true) {
                try {
                    int index = rnd.Next(0, 10);
                    temperatures[index] += rnd.NextDecimal();

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _eventAggregator.GetEvent<TemperatureChangeEvent>().Publish(new TemperatureChange { Index = index + 1, Value = temperatures[index] });
                    });
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
