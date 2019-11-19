using Windows.ApplicationModel.Background;

namespace Attention.UWP.Background
{
    public sealed class LiveTitleBackgroundExecution : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var d = taskInstance.GetDeferral();

            /*
             * TODO:
             * 
             *Documentation:
             *   General: https://docs.microsoft.com/en-us/windows/uwp/launch-resume/support-your-app-with-background-tasks
             *   Debug: https://docs.microsoft.com/en-us/windows/uwp/launch-resume/debug-a-background-task
             *   Monitoring: https://docs.microsoft.com/windows/uwp/launch-resume/monitor-background-task-progress-and-completion
             */

            d.Complete();
        }
    }
}
