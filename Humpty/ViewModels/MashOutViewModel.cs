using System;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace Humpty.ViewModels {
    public class MashOutViewModel : DisplayEventHandlerViewModelBase {

        public MashOutViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
