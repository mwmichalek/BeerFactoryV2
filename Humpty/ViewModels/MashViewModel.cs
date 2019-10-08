using System;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace Humpty.ViewModels {
    public class MashViewModel : DisplayEventHandlerViewModelBase {
        public MashViewModel() {
        }

        public MashViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
