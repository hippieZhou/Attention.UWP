using Attention.Core.Tools;
using Windows.ApplicationModel.Background;

namespace Attention.BackgroundTask
{
    public sealed class BackgroundExecution : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += (sender, e) => 
            {
                //todo
            };

            await BackgroundTaskAction.Update();

            deferral.Complete();
        }
    }
}
