using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Mwm.BeerFactoryV2.Uwp.Tips.BackgroundTasks {
    public interface IBackgroundTask {

        void Register();

        Task RunAsyncInternal(IBackgroundTaskInstance taskInstance);

        void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

        bool Match(string name);

        Task RunAsync(IBackgroundTaskInstance taskInstance);

        void SubscribeToEvents(IBackgroundTaskInstance taskInstance);

    }
}
