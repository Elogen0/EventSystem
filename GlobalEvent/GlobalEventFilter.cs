using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Apply the effect of the event by comparing the filter in Listener with the filter in Spec
    /// </summary>
    public enum GlobalEventFilter : uint
    {
        None        = 0,
        Object      = 0b11111111,
        User        = 0b00000001,
        Enemy       = 0b00000010,
        World       = 0b1111111100000000,
        All         = 0xFFFFFFFF
    }
}
