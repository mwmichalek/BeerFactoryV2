using System;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace Humpty.ViewModels {
    public class SpargeViewModel : DisplayEventHandlerViewModelBase {
        public SpargeViewModel() {
        }

        public SpargeViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
