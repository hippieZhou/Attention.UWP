using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace HENG.Tasks
{
    public sealed class DefaultBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            RunBackgroundTask();

            _deferral.Complete();
        }

        private void RunBackgroundTask()
        {

        }
    }
}
