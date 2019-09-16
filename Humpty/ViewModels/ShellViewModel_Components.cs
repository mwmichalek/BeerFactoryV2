using System;
using System.Linq;
using System.Windows.Input;

using Humpty.Helpers;
using Humpty.Views;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Serilog;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace Humpty.ViewModels {
    public partial class ShellViewModel : DisplayEventHandlerViewModelBase {

        public override void SsrChangeOccured(SsrChange ssrChange) {
            if (ssrChange.Id == SsrId.HLT) {
                HltElement1FillColor = ssrChange.IsEngaged ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Black);
            } else {
                BkElement1FillColor = ssrChange.IsEngaged ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Black);
            }
        }

        private Brush _hltElement1FillColor;

        public Brush HltElement1FillColor {
            get { return _hltElement1FillColor; }
            set {
                SetProperty(ref _hltElement1FillColor, value);
            }
        }

        private Brush _bkElement1FillColor;

        public Brush BkElement1FillColor {
            get { return _bkElement1FillColor; }
            set {
                SetProperty(ref _bkElement1FillColor, value);
            }
        }

    }
}
