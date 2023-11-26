using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Job called from the Scheduler at the start of the event
    /// </summary>
    public class GlobalEventStartJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string eventName = context.JobDetail.JobDataMap.Get("eventId") as string;
            if (!int.TryParse(eventName, out int eventId))
                return Task.CompletedTask;

            GlobalEventSpec globalEvent = GlobalEventManager.Instance.GetEvent(eventId);
            if (globalEvent == null)
                return Task.CompletedTask;

            // If the current time is within the GameEvent's time range, start the event
            if (!globalEvent.IsStarted && DateTime.Now >= globalEvent.StartTime && DateTime.Now <= globalEvent.EndTime)
            {
                GlobalEventManager.Instance.OnStartEvent(globalEvent);
            }

            return Task.CompletedTask;
        }
    }


    /// <summary>
    /// Job called from the Scheduler at the end of the event
    /// </summary>
    public class GlobalEventEndJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            // Fetch the GameEvent from the JobDataMap
            string eventName = context.JobDetail.JobDataMap.Get("eventId") as string;
            if (!int.TryParse(eventName, out int eventId))
                return Task.CompletedTask;

            GlobalEventSpec globalEvent = GlobalEventManager.Instance.GetEvent(eventId);
            if (globalEvent == null)
                return Task.CompletedTask;
            if (globalEvent.IsStarted && DateTime.Now >= globalEvent.EndTime)
            {
                GlobalEventManager.Instance.OnEndEvent(globalEvent);
            }

            return Task.CompletedTask;
        }
    }
}
