using GameServerAlpha.GameLogic.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Parameters to be transferred when an event occurs
    /// </summary>
    public class GlobalEventParam
    {
        public GlobalEventParamType Type { get; set; }
        public int Id { get; set; }
        public int Value { get; set; }
        public object Option { get; set; }
    }
}
