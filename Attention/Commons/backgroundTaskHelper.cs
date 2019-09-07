using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Attention.Commons
{
    public class backgroundTaskHelper
    {
        private const string backgroundTaskName = "LiveTitleService";

        public static async Task InitializeComponentAsync()
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered(backgroundTaskName))
            {
                return;
            }
            else
            {
                await BackgroundExecutionManager.RequestAccessAsync();
                BackgroundTaskRegistration registered = BackgroundTaskHelper.Register(
                    backgroundTaskName,
                    new TimeTrigger(15, true), false, true,
                    new SystemCondition(SystemConditionType.InternetAvailable));
            }
        }
    }
}
