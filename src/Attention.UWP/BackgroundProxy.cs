using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;

namespace Attention.UWP
{
    public class BackgroundProxy
    {
        private const string wallpaperBackgroundTaskName = "WallpaperBackgroundTaskName";

        public async void Register()
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered(wallpaperBackgroundTaskName))
            {
                return;
            }

            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if (access == BackgroundAccessStatus.DeniedBySystemPolicy
               || access == BackgroundAccessStatus.DeniedByUser)
            {
                await new MessageDialog("系统关闭了后台运行，请前往‘系统设置’进行设置").ShowAsync();
                return;
            }

            BackgroundTaskHelper.Register(wallpaperBackgroundTaskName,
                typeof(Background.LiveTitleBackgroundExecution).FullName,
                new TimeTrigger(15, false),
                false, true,
                new SystemCondition(SystemConditionType.InternetAvailable));
        }

        public void UnRegister() => BackgroundTaskHelper.Unregister(wallpaperBackgroundTaskName);
    }
}
