using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent.Test
{
    public class TestGobalEventCommand : IGlobalEventCommand
    {
        public GlobalEventCommandType CommandType => GlobalEventCommandType.Test;

        public void OnExecuteStartEvent(IGlobalEventListener listener, List<GlobalEventParam> paramList)
        {
            TestContext.WriteLine("StartEvent");
            listener.DoSomthing("start");
            foreach (var param in paramList)
            {
                listener.TakeParam(param.Id, param.Value);
            }
        }

        public void OnExecuteEndEvent(IGlobalEventListener listener, List<GlobalEventParam> paramList)
        {
            TestContext.WriteLine("EndEvent");
            listener.DoSomthing("end");
        }

        
    }
}
