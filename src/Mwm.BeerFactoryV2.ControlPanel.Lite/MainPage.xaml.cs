using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        //STEP 2: Fill Hot Liquor Tank with water
        //STEP 3: Heat strike water
        //STEP 4: Transfer strike water to Mash/Lauter Tun
        //STEP 5: Mash
        //STEP 6: Mash-out
        //STEP 7: Sparge
        //STEP 8: Boil
        //STEP 9: Chill

        public ObservableCollection<TemperatureReading> TemperatureReadings = new ObservableCollection<TemperatureReading>();





        public MainPage() {
            this.InitializeComponent();
        }
    }
}
