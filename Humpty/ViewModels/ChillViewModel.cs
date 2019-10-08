using System;
using Prism.Events;
using Prism.Windows.Mvvm;

namespace Humpty.ViewModels
{
    public class ChillViewModel : DisplayEventHandlerViewModelBase {
        public ChillViewModel() {
        }

        public ChillViewModel(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
