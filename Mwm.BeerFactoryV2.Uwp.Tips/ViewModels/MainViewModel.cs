using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using Prism.Windows.Mvvm;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Mwm.BeerFactoryV2.Uwp.Tips.ViewModels {
    public class MainViewModel : BeerFactoryViewModelBase {

        public MainViewModel(IBeerFactory beerfactor) : base(beerfactor) {

        }

    }
}
