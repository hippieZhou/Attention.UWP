using Attention.BackgroundTask;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;

namespace Attention
{
    public class BackgroundProxy
    {
        public async void Register()
        {
            BackgroundExecutionManager.RemoveAccess();
            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if (access == BackgroundAccessStatus.DeniedBySystemPolicy || access == BackgroundAccessStatus.DeniedByUser || access == BackgroundAccessStatus.Unspecified)
            {
                await new MessageDialog("系统关闭了后台运行，请前往‘系统设置’进行设置").ShowAsync();
                return;
            }
            RegisterLiveTitleBackgroundTask();
        }

        /// <summary>
        /// MPM
        /// </summary>
        private void RegisterLiveTitleBackgroundTask()
        {
            BackgroundTaskRegistration registered =
                BackgroundTaskHelper.Register(typeof(BackgroundExecution),
                new TimeTrigger(15, true),
                false, true,
                new SystemCondition(SystemConditionType.InternetAvailable),
                new SystemCondition(SystemConditionType.UserPresent));

            if (registered != null)
            {
                Trace.WriteLine($"Task {typeof(BackgroundExecution)} registered successfully.");
            }
        }
    }
}
