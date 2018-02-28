using System;
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
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mwm.BeerFactoryV2.ControlPanel.Lite {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;
        private ObservableCollection<DeviceInformation> listOfDevices;
        private CancellationTokenSource ReadCancellationTokenSource;

        public MainPage() {
            InitializeComponent();
            listOfDevices = new ObservableCollection<DeviceInformation>();
            ListAvailablePorts();
            SerialPortConfiguration();
        }

        private async void ListAvailablePorts() {
            try {
                string deviceSelector = SerialDevice.GetDeviceSelector();
                var deviceInfos = await DeviceInformation.FindAllAsync(deviceSelector);
                //for (int i = 0; i < deviceInfos.Count; i++) {

                //    listOfDevices.Add(deviceInfos[i]);
                //    Debug.WriteLine(deviceInfos[i]);
                //}

                foreach (var deviceInfo in deviceInfos) {
                    listOfDevices.Add(deviceInfo);
                    Debug.WriteLine($"Id: {deviceInfo.Id} Name:{deviceInfo.Name} Other: {deviceInfo.Kind}");
                }

                    
                
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
        }

        //private async void ButtonClick(object sender, RoutedEventArgs e) {
        //    var buttonClicked = sender as Button;
        //    switch (buttonClicked.Name) {
        //        case "btnSerialConnect":
        //            SerialPortConfiguration();
        //            break;
        //        case "btnSerialDisconnect":
        //            SerialPortDisconnect();
        //            break;
        //        case "btnAccendiled":
        //            if (serialPort != null) {
        //                dataWriteObject = new DataWriter(serialPort.OutputStream);
        //                await ManageLed("2");
        //            }
        //            if (dataWriteObject != null) {
        //                dataWriteObject.DetachStream();
        //                dataWriteObject = null;
        //            }
        //            break;
        //        case "btnSpegniled":
        //            if (serialPort != null) {
        //                dataWriteObject = new DataWriter(serialPort.OutputStream);
        //                await ManageLed("1");
        //            }
        //            if (dataWriteObject != null) {
        //                dataWriteObject.DetachStream();
        //                dataWriteObject = null;
        //            }
        //            break;
        //        case "btnPulse1000ms":
        //            if (serialPort != null) {
        //                dataWriteObject = new DataWriter(serialPort.OutputStream);
        //                await ManageLed("3");
        //            }
        //            if (dataWriteObject != null) {
        //                dataWriteObject.DetachStream();
        //                dataWriteObject = null;
        //            }
        //            break;
        //        case "btnPulse2000ms":
        //            if (serialPort != null) {
        //                dataWriteObject = new DataWriter(serialPort.OutputStream);
        //                await ManageLed("4");
        //            }
        //            if (dataWriteObject != null) {
        //                dataWriteObject.DetachStream();
        //                dataWriteObject = null;
        //            }
        //            break;
        //    }
        //}

        private async void SerialPortConfiguration() {

            DeviceInformation entry = (DeviceInformation)listOfDevices[0];
            try {
                serialPort = await SerialDevice.FromIdAsync(entry.Id);
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;
                ReadCancellationTokenSource = new CancellationTokenSource();
                Listen();
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
        }

        private void SerialPortDisconnect() {
            try {
                CancelReadTask();
                CloseDevice();
                ListAvailablePorts();
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
        }

        private async Task SendCommand(string command) {

            Task<UInt32> storeAsyncTask;
            if (command.Length != 0) {
                
                dataWriteObject.WriteString(command);
                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();
                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0) {
                    Debug.WriteLine("Wrote message");
                }
            } 
        }

        private async void Listen() {
            try {
                if (serialPort != null) {
                    dataReaderObject = new DataReader(serialPort.InputStream);
                    dataWriteObject = new DataWriter(serialPort.OutputStream);
                    while (true) {
                        await ReadData(ReadCancellationTokenSource.Token);
                    }
                }
            } catch (Exception ex) {

                if (ex.GetType().Name == "TaskCanceledException") {
                    CloseDevice();
                } else {
                    //tbkAllarmi.Text = "Task annullato";
                }
            } finally {
                if (dataReaderObject != null) {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
                if (dataWriteObject != null) {
                    dataWriteObject.DetachStream();
                    dataWriteObject = null;
                }
            }
        }

        private async Task ReadData(CancellationToken cancellationToken) {
            Task<UInt32> loadAsyncTask;
            uint ReadBufferLength = 1024;
            cancellationToken.ThrowIfCancellationRequested();
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0) {
                Debug.WriteLine(dataReaderObject.ReadString(bytesRead));
                //tbkStatusLed.Text = dataReaderObject.ReadString(bytesRead);
            }
        }

        private void CancelReadTask() {
            if (ReadCancellationTokenSource != null) {
                if (!ReadCancellationTokenSource.IsCancellationRequested) {
                    ReadCancellationTokenSource.Cancel();
                }
            }
        }

        private void CloseDevice() {
            if (serialPort != null) {
                serialPort.Dispose();
            }
            serialPort = null;
            listOfDevices.Clear();
        }

        private async void SendShit_Click(object sender, RoutedEventArgs e) {
            await SendCommand($"echo {ShitMessage.Text}");
        }
    }

}
