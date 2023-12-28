using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    public class TimerInitEvent : PubSubEvent
    {
    }

    public class ShutDownComputerEvent : PubSubEvent
    { }

    public class UpdateTimerEvent : PubSubEvent<Tuple<string, string, string>>
    { }

    public class UpdateIsWorkingEvent : PubSubEvent<bool>
    { }

    public class WindowCloseEvent : PubSubEvent
    { }

    public class WindowShowEvent : PubSubEvent
    { }

    public class WindowHideEvent : PubSubEvent
    { }

    public class WindowTipEvent : PubSubEvent
    { }

    public class UpdateAppStartEvent : PubSubEvent
    { }

    public class UpdateAppEndEvent : PubSubEvent<bool>
    { }
}