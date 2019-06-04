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

        public void StrikeWaterSetPointPublishChangeEvent() {
            //_eventAggregator.GetEvent<SsrChangeEvent>().Publish(new KettleCommand { Index = 1, Percentage = hltPercentageSetting });
        }

        public void MashSetPointPublishChangeEvent() {
            //throw new NotImplementedException();
        }

        public void BoilKettleSetPointPublishChangeEvent() {
            //throw new NotImplementedException();
        }
    }
}
