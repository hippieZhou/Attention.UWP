using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace HENG.Services
{
    /// <summary>
    /// https://github.com/lbugnion/mvvmlight/blob/master/GalaSoft.MvvmLight/GalaSoft.MvvmLight.Platform%20(UWP)/Views/NavigationService.cs
    /// </summary>
    public class ExNavigationService : INavigationService
    {
        public const string RootControlKey = "-- ROOT --";
        public const string UnknownControlKey = "-- UNKNOWN --";

        private readonly Dictionary<string, Type> _controlsByKey = new Dictionary<string, Type>();
        private readonly Stack<string> _controlStacks = new Stack<string>();

        private ContentControl _contentControl;
        public ContentControl ContentControl
        {
            get { return _contentControl; }
            set { _contentControl = value; }
        }

        public string CurrentPageKey
        {
            get
            {
                lock (_controlsByKey)
                {
                    if (_controlStacks.Count == 0)
                    {
                        return RootControlKey;
                    }

                    if (ContentControl == null)
                    {
                        return UnknownControlKey;
                    }

                    var currentType = ContentControl.Content.GetType();
                    if (_controlsByKey.All(p => p.Value != currentType))
                    {
                        return UnknownControlKey;
                    }

                    var item = _controlsByKey.FirstOrDefault(i => i.Value == currentType);
                    return item.Key;
                }
            }
        }

        public void Configure(string key, Type pageType)
        {
            lock (_controlsByKey)
            {
                if (_controlsByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }
                if (_controlsByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + _controlsByKey.First(p => p.Value == pageType).Key);
                }

                _controlsByKey.Add(
                    key,
                    pageType);
            }
        }

        public void GoBack()
        {
            if (_controlStacks.Count > 0)
            {
                ContentControl.Content = _controlsByKey[_controlStacks.Pop()];
            }
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_controlsByKey)
            {
                if (!_controlsByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }
                ContentControl.Content = _controlsByKey[pageKey];
                _controlStacks.Push(pageKey);
            }
        }
    }
}
