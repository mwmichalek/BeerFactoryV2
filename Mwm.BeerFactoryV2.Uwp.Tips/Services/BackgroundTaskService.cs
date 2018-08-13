using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Mwm.BeerFactoryV2.Uwp.Tips.BackgroundTasks;
using Mwm.BeerFactoryV2.Uwp.Tips.Helpers;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace Mwm.BeerFactoryV2.Uwp.Tips.Services {
    public class BackgroundTaskService : IBackgroundTaskService {

        public static IList<BackgroundTasks.IBackgroundTask> BackgroundTasks = new List<BackgroundTasks.IBackgroundTask>();

        public async Task RegisterBackgroundTasksAsync(IEnumerable<BackgroundTasks.IBackgroundTask> tasks) {
            BackgroundExecutionManager.RemoveAccess();
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.DeniedBySystemPolicy
                || result == BackgroundAccessStatus.DeniedByUser) {
                return;
            }
            
            foreach (var task in tasks) {
                BackgroundTasks.Add(task);
                task.Register();
            }
        }

        public void Start(IBackgroundTaskInstance taskInstance) {
            var task = BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance?.Task?.Name));

            if (task == null) {
                // This condition should not be met. It is it it means the background task to start was not found in the background tasks managed by this service.
                // Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                return;
            }

            task.RunAsync(taskInstance).FireAndForget();
        }

        //private static IEnumerable<BackgroundTask> CreateInstances() {
        //    var backgroundTasks = new List<BackgroundTask>();

        //    //backgroundTasks.Add(new ArduinoBackgroundTask());
        //    return backgroundTasks;
        //}
    }
}
