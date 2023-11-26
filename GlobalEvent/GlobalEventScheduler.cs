using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    public class GlobalEventScheduler
    {
        private readonly IScheduler _scheduler;

        public GlobalEventScheduler()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            _scheduler.Start();
        }

        public async Task ScheduleEvent(GlobalEventSpec globalEvent)
        {
            IJobDetail startJob = JobBuilder.Create<GlobalEventStartJob>()
                .WithIdentity($"{globalEvent.Id}_start", "globalEvents")
                .UsingJobData($"eventId", globalEvent.Id.ToString())
                .Build();

            ITrigger startTrigger = TriggerBuilder.Create()
                .WithIdentity($"{globalEvent.Id}_start", "globalEvents")
                .StartAt(globalEvent.StartTime)
                .Build();

            await _scheduler.ScheduleJob(startJob, startTrigger);

            IJobDetail endJob = JobBuilder.Create<GlobalEventEndJob>()
                .WithIdentity($"{globalEvent.Id}_end", "globalEvents")
                .UsingJobData($"eventId", globalEvent.Id.ToString())
                .Build();

            ITrigger endTrigger = TriggerBuilder.Create()
                .WithIdentity($"{globalEvent.Id}_end", "globalEvents")
                .StartAt(globalEvent.EndTime)
                .Build();

            await _scheduler.ScheduleJob(endJob, endTrigger);
        }

        public async Task UnscheduleEvent(GlobalEventSpec globalEvent)
        {
            TriggerKey startTriggerKey = new TriggerKey($"{globalEvent.Id}_start", "globalEvents");
            if (startTriggerKey != null)
            {
                await _scheduler.UnscheduleJob(startTriggerKey);
            }

            TriggerKey endTriggerKey = new TriggerKey($"{globalEvent.Id}_end", "globalEvents");
            if (endTriggerKey != null)
            {
                await _scheduler.UnscheduleJob(endTriggerKey);
            }
        }
        
    }
}
