using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Implement this to receive an event
    /// </summary>
    public interface IGlobalEventListener
    {
        GlobalEventFilter Filter { get; }
        void DoSomthing(string message); //test
        void TakeParam(int id, int value); //test
    }
}
