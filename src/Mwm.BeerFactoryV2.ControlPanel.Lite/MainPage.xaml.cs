﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.Firmata;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mwm.BeerFactoryV2.ControlPanel.Lite {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        private UwpFirmata firmata;
        private UsbSerial connection;
        private RemoteDevice arduino;

        private int heater1 = 0;
        private int heater2 = 0;
        private int pump1 = 0;
        private int pump2 = 0;

        private double temp1 = double.MinValue;
        private double temp2 = double.MinValue;
        private double temp3 = double.MinValue;

        //public ObservableCollection<TemperatureReading> TemperatureReadings = new ObservableCollection<TemperatureReading>();

        public MainPage() {
            this.InitializeComponent();
        

            //USB\VID_2341 & PID_0042 & REV_0001

            connection = new UsbSerial("VID_2341", "PID_0042");   // COM3
            //connection = new UsbSerial("VID_2341", "PID_0043");     // COM4

            firmata = new UwpFirmata();
            arduino = new RemoteDevice(firmata);

            connection.ConnectionEstablished += OnConnectionEstablished;
       
            firmata.StringMessageReceived += OnStringMessageReceived;
            firmata.FirmataConnectionReady += OnFirmataConnectionReady;
            firmata.FirmataConnectionFailed += OnFirmataConnectionFailed;
            firmata.FirmataConnectionLost += OnFirmataConnecitonLost;

            Connect();

            Task.Run(() => Ping());
        }

        private void Connect() {
            connection.begin(115200, SerialConfig.SERIAL_8N1);
            firmata.begin(connection);
        }

        private async void OnStringMessageReceived(UwpFirmata caller, StringCallbackEventArgs argv) {
            try {
                var settingStr = argv.getString().Split('=');
                var settingName = settingStr[0];
                var settingValue = settingStr[1];
                Debug.WriteLine($"Received: {settingName} = {settingValue}");

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => {
                    //if (settingName == "T1") {
                    //    temp1 = double.Parse(settingValue);
                    //        //tempGauge1.MainScale.Pointers[0].Value = temp1;
                    //        //tempDigital1.Value = temp1.ToString("00.00");
                    //    } else if (settingName == "T2") {
                    //    temp2 = double.Parse(settingValue);
                    //        //tempGauge2.MainScale.Pointers[0].Value = temp2;
                    //        //tempDigital2.Value = temp2.ToString("00.00");
                    //    } else if (settingName == "T3") {
                    //    temp3 = double.Parse(settingValue);
                    //        //tempGauge3.MainScale.Pointers[0].Value = temp3;
                    //        //tempDigital3.Value = temp3.ToString("00.00");
                    //    } else if (settingName == "CFG") {
                    //    Task.Run(PushSettings);
                    //}
                });

            } catch (Exception) { }
        }

        private void OnFirmataConnecitonLost(string message) {
            Debug.WriteLine("OnFirmataConnecitonLost.");
        }

        private void OnFirmataConnectionFailed(string message) {
            Debug.WriteLine("OnFirmataConnectionFailed.");
        }

        private void OnFirmataConnectionReady() {
            Debug.WriteLine("OnFirmataConnectionReady.");
        }

        private void OnConnectionEstablished() {
            Debug.WriteLine("OnConnectionEstablished.");
        }


        private void Ping() {
            while (true) {
                var cmd = $"{DateTime.Now.ToString("HH:mm:ss")}";
                Debug.WriteLine("\t" + cmd);
                firmata.sendString(cmd);
                firmata.flush();
                Task.Delay(1000).Wait();
            }
        }

        private async Task PushSettings() {
            await Task.Run(() => {
                var msg = $"{heater1.ToString("D3")}:{heater2.ToString("D3")}:{pump1}:{pump2}:{DateTime.Now.ToString("h:mm:ss")}";
                firmata.sendString(msg);
                firmata.flush();
                Debug.WriteLine($"Sent message to Arduino: '{msg}'.");
            });
        }
    }
}
