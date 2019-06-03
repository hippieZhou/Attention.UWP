using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Windows.ApplicationModel.Background;
using System.Collections.Generic;

namespace HENG.Helpers
{
    public class BackgroundTaskHelper
    {
        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(Type taskEntryPoint, string taskName, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser || status == BackgroundAccessStatus.DeniedBySystemPolicy)
            {
                return null;
            }
            var tasks = BackgroundTaskRegistration.AllTasks.Values.Where(t => t.Name == taskName);
            tasks.AsParallel().ForAll(p => { p.Unregister(true); });

            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = taskEntryPoint.FullName
            };

            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            Trace.WriteLine($"Task {taskName} registered successfully.");

            return task;
        }
    }
}
