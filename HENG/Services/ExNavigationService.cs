using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace HENG.Services
{
    /// <summary>
    /// https://github.com/lbugnion/mvvmlight/blob/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight.Platform%20(UWP)/Views/NavigationService.cs
    /// </summary>
    public class ExNavigationService : INavigationService
    {
        public const string RootPageKey = "-- ROOT --";
        public const string UnknownPageKey = "-- UNKNOWN --";

        /// <summary>
        /// Stack
        /// </summary>
        private readonly Dictionary<string, Type> _controlsByKey = new Dictionary<string, Type>();
        private readonly Stack<Type> _controls = new Stack<Type>();


        private ContentControl _currentControl;
        public ContentControl CurrentControl
        {
            get { return _currentControl; }
            set { _currentControl = value; }
        }


        public string CurrentPageKey => throw new NotImplementedException();

        public void GoBack()
        {
            if (_controlsByKey.Count > 0)
            {
            }
        }

        public void NavigateTo(string pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
