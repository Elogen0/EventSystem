using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Add a type according to the command to dynamically specify the behavior of the event
    /// </summary>
    public enum GlobalEventCommandType
    {
        Test,
        AnnualEvent,
        AttendanceCheck,
        RewardAtKnight,
        UserReturnMonthly,
    }
}
