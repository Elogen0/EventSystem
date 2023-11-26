using GameServerAlpha.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Responsible for the action of the event
    /// </summary>
    public interface IGlobalEventCommand
    {
        GlobalEventCommandType CommandType { get; }
        void OnExecuteStartEvent(IGlobalEventListener listener, List<GlobalEventParam> paramList);
        void OnExecuteEndEvent(IGlobalEventListener listener, List<GlobalEventParam> paramList);
    }
}
