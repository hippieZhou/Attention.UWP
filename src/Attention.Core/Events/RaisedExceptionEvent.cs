using Prism.Events;
using System;

namespace Attention.Core.Events
{
    public class RaisedExceptionEvent : PubSubEvent<Exception>
    {
    }
}
