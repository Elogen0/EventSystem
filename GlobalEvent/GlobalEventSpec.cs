using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Define the event's specifications
    /// </summary>
    public class GlobalEventSpec
    {
        public int Id { get; set; }
        public GlobalEventFilter Filter { get; set; } = GlobalEventFilter.All;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<GlobalEventParam> Params { get; set; } = new List<GlobalEventParam>();
        public GlobalEventCommandType CommandType { get; set; }
        public bool IsStarted { get; set; } = false;

        public class Builder
        {
            private readonly GlobalEventSpec _spec = new GlobalEventSpec();

            public GlobalEventSpec Build() => _spec;

            public Builder(int id)
            {
                _spec.Id = id;
            }

            public Builder SetFilter (GlobalEventFilter filter)
            {
                _spec.Filter = filter;
                return this;
            }

            public Builder SetTime(DateTime startTime, DateTime endTime)
            {
                _spec.StartTime = startTime;
                _spec.EndTime = endTime;
                return this;
            }

            public Builder AddParam(GlobalEventParam param) {
                _spec.Params.Add(param);
                return this;
            }

            public Builder AddParam(IEnumerable<GlobalEventParam> paramList)
            {
                foreach (GlobalEventParam effect in paramList)
                { 
                    _spec.Params.Add(effect); 
                }
                return this;
            }

            public Builder SetCommandType(GlobalEventCommandType commandType)
            {
                _spec.CommandType = commandType;
                return this;
            }
        }
    }
}
