using Prism.Events;
using System;

namespace Attention.App.Events
{
    public class RaisedExceptionEvent : PubSubEvent<Exception>
    {
    }
}
