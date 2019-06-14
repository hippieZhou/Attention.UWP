using System;
using System.Collections.Concurrent;

namespace HENG.Core.Helpers
{
    public class SingletonProvider<T> where T : new()
    {
        public SingletonProvider() { }
        public static T Instance => SingletonCreator.instance;

        class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T instance = new T();
        }
    }
}
