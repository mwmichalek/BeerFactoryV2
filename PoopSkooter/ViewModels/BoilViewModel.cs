using System;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace PoopSkooter.ViewModels {
    public class BoilViewModel : DisplayEventHandlerViewModelBase {
        public BoilViewModel() {
        }

        public BoilViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
