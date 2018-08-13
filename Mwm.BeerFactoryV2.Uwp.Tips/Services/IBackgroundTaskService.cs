using Microsoft.Practices.Unity;
using Mwm.BeerFactoryV2.Uwp.Tips.BackgroundTasks;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Mwm.BeerFactoryV2.Uwp.Tips.Services
{
    internal interface IBackgroundTaskService
    {
        Task RegisterBackgroundTasksAsync(IEnumerable<IBackgroundTask> backgroundTasks);

        void Start(Windows.ApplicationModel.Background.IBackgroundTaskInstance taskInstance);
    }
}
