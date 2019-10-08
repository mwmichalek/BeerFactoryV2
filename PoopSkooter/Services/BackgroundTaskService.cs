using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PoopSkooter.BackgroundTasks;
using PoopSkooter.Core.Helpers;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace PoopSkooter.Services
{
    internal class BackgroundTaskService : IBackgroundTaskService
    {
        public static IEnumerable<BackgroundTask> BackgroundTasks => BackgroundTaskInstances.Value;

        private static readonly Lazy<IEnumerable<BackgroundTask>> BackgroundTaskInstances =
            new Lazy<IEnumerable<BackgroundTask>>(() => CreateInstances());

        public async Task RegisterBackgroundTasksAsync()
        {
            BackgroundExecutionManager.RemoveAccess();
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.DeniedBySystemPolicy
                || result == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (var task in BackgroundTasks)
            {
                task.Register();
            }
        }

        public void Start(IBackgroundTaskInstance taskInstance)
        {
            var task = BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance?.Task?.Name));

            if (task == null)
            {
                // This condition should not be met. It is it it means the background task to start was not found in the background tasks managed by this service.
                // Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                return;
            }

            task.RunAsync(taskInstance).FireAndForget();
        }

        private static IEnumerable<BackgroundTask> CreateInstances()
        {
            var backgroundTasks = new List<BackgroundTask>();

            backgroundTasks.Add(new ArduinoTask());
            return backgroundTasks;
        }
    }
}
