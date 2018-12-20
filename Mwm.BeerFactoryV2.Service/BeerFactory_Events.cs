using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service {

    public partial class BeerFactory {

        private IEventManager _eventManager;

        public void ConfigureEvents() {

            _eventManager.Subscribe<TemperatureChange>((temperatureResult) => {
                var thermometer = _thermometers.SingleOrDefault(t => (int)t.Id == temperatureResult.Index);
                if (thermometer != null) {
                    thermometer.Temperature = temperatureResult.Value;
                    thermometer.Timestamp = DateTime.Now;
                }
            });

            _eventManager.Subscribe<ConnectionStatus>((connectionStatus) => {
                Debug.WriteLine($"Connection Status: {connectionStatus.Type}");
                //ConnectionStatus = $"{connectionStatus.Type}";
            });

            _eventManager.Subscribe<SsrChange>((ssrResult) => {
                //Debug.WriteLine($"SSR Status: {ssrResult.Index} {ssrResult.IsEngaged}");
                if (ssrResult.Index == 1) {
                    //HltElementEngagedBrush = ssrResult.IsEngaged ? yellow : black;
                    //HltPercentage = ssrResult.Percentage;
                } else if (ssrResult.Index == 2) {
                    //BkElementEngagedBrush = ssrResult.IsEngaged ? yellow : black;
                    //BkPercentage = ssrResult.Percentage;
                }
            });

            //_eventAggregator.GetEvent<KettleResultEvent>().Subscribe((kettleResult) => {
            //    Debug.WriteLine($"Kettle Shit Happened: {kettleResult.Index} {kettleResult.Percentage}");
            //    if (kettleResult.Index == 1) {
            //        //Debug.WriteLine($"HLT Percentage: {kettleResult.Percentage}");
            //        //HltPercentage = kettleResult.Percentage;
            //    } else if (kettleResult.Index == 2) {
            //        //Debug.WriteLine($"BK Percentage: {kettleResult.Percentage}");
            //        //BkPercentage = kettleResult.Percentage;
            //    }
            //});

            _eventManager.Subscribe<Message>((message) => {
                Debug.WriteLine($"Message: {message.Index} {message.Body}");

                if (message.Index == 1) {
                    //Debug.WriteLine($"HLT Percentage: {kettleResult.Percentage}");
                    //HltPercentage = message.Percentage;
                } else if (message.Index == 2) {
                    //Debug.WriteLine($"BK Percentage: {kettleResult.Percentage}");
                    //BkPercentage = message.Percentage;
                }
            });
        }

    }
}
