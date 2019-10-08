using System;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace Humpty.ViewModels {
    public class StrikeWaterTransferViewModel : DisplayEventHandlerViewModelBase {
        public StrikeWaterTransferViewModel() {
        }

        public StrikeWaterTransferViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
