﻿namespace Attention.Core.Framework
{
    public interface IEngine
    {
        T Resolve<T>() where T : class;
        T Resolve<T>(string name) where T : class;
    }
}
