using Mwm.BeerFactoryV2.Service.Events;
using Mwm.BeerFactoryV2.Service.Phases;
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

        private IEventAggregator _eventAggregator;

        public void ConfigureEvents() {

            _eventAggregator.GetEvent<TemperatureResultEvent>().Subscribe((temperatureResult) => {
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

            _eventAggregator.GetEvent<SsrResultEvent>().Subscribe((ssrResult) => {
                //Debug.WriteLine($"SSR Status: {ssrResult.Index} {ssrResult.IsEngaged}");
                if (ssrResult.Index == 1) {
                    HltElementEngagedBrush = ssrResult.IsEngaged ? yellow : black;
                    HltPercentage = ssrResult.Percentage;
                } else if (ssrResult.Index == 2) {
                    BkElementEngagedBrush = ssrResult.IsEngaged ? yellow : black;
                    BkPercentage = ssrResult.Percentage;
                }
            });

            _eventAggregator.GetEvent<KettleResultEvent>().Subscribe((kettleResult) => {
                Debug.WriteLine($"Kettle Shit Happened: {kettleResult.Index} {kettleResult.Percentage}");
                if (kettleResult.Index == 1) {
                    //Debug.WriteLine($"HLT Percentage: {kettleResult.Percentage}");
                    HltPercentage = kettleResult.Percentage;
                } else if (kettleResult.Index == 2) {
                    //Debug.WriteLine($"BK Percentage: {kettleResult.Percentage}");
                    BkPercentage = kettleResult.Percentage;
                }
            });

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

    }
}
