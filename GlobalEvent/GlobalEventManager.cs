using GameServerAlpha.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// GlobalEvent API
    /// </summary>
    public class GlobalEventManager
    {
        public static GlobalEventManager Instance { get; } = new GlobalEventManager();

        private Dictionary<int, GlobalEventSpec> _allEvents = new Dictionary<int, GlobalEventSpec>();
        private List<GlobalEventSpec> _onGoingEvents = new List<GlobalEventSpec>();
        private List<IGlobalEventListener> _listeners = new List<IGlobalEventListener>();
        private GlobalEventScheduler _scheduler = new GlobalEventScheduler();
        private Dictionary<GlobalEventCommandType, IGlobalEventCommand> _commands;
        
        private GlobalEventManager() { }

        public void AddListener(IGlobalEventListener listener)
        {
            _listeners.Add(listener);
            
            foreach (var onGoingEvent in _onGoingEvents)
            {
                if (_commands.TryGetValue(onGoingEvent.CommandType, out var command))
                {
                    command.OnExecuteStartEvent(listener, onGoingEvent.Params);
                }
            }
        }

        public void RemoveListener(IGlobalEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public async Task AddEvent(GlobalEventSpec eventSpec)
        {
            if (_allEvents.ContainsKey(eventSpec.Id))
            {
                Trace.WriteLine($"{eventSpec.Id} is already added");
                return;
            }
            if (DateTime.Now > eventSpec.EndTime)
                return;

            _allEvents.Add(eventSpec.Id, eventSpec);

            if (DateTime.Now >= eventSpec.StartTime && DateTime.Now <= eventSpec.EndTime)
            {
                OnStartEvent(eventSpec);
            }
            else
            {
                await _scheduler.ScheduleEvent(eventSpec);
            }
        }

        public async Task CancelEvent(int eventId)
        {
            if (_allEvents.TryGetValue(eventId, out GlobalEventSpec eventSpec))
            {
                await OnEndEvent(eventSpec);
            }
        }

        public async Task ClearEvents()
        {
            foreach (var eventSpec in _allEvents.Values)
            {
                await OnEndEvent(eventSpec);
            }
        }

        public GlobalEventSpec GetEvent(int eventId)
        {
            if (_allEvents.TryGetValue(eventId, out GlobalEventSpec globalEvent))
            {
                return globalEvent;
            }
            return null;
        }

        public void InitCommand(Dictionary<GlobalEventCommandType, IGlobalEventCommand> commands)
        {
            _commands = commands;
        }

        #region Internal Callbacks
        public void OnStartEvent(GlobalEventSpec eventSpec)
        {
            eventSpec.IsStarted = true;
            _onGoingEvents.Add(eventSpec);
            foreach (var listener in _listeners)
            {
                if ((listener.Filter & eventSpec.Filter) == GlobalEventFilter.None)
                    continue;
                if (_commands.TryGetValue(eventSpec.CommandType, out var command))
                {
                    command.OnExecuteStartEvent(listener, eventSpec.Params);
                }
            }
        }

        public async Task OnEndEvent(GlobalEventSpec eventSpec)
        {
            await RemoveEvent(eventSpec.Id);
            eventSpec.IsStarted = false;
            
            foreach (var listener in _listeners)
            {
                if ((listener.Filter & eventSpec.Filter) == GlobalEventFilter.None)
                    continue;
                if (_commands.TryGetValue(eventSpec.CommandType, out var command))
                {
                    command.OnExecuteEndEvent(listener, eventSpec.Params);
                }
            }
        }
        #endregion

        private async Task<bool> RemoveEvent(int eventId)
        {
            if (_allEvents.TryGetValue(eventId, out GlobalEventSpec eventSpec))
            {
                _allEvents.Remove(eventId);
                if (_onGoingEvents.Remove(eventSpec))
                {
                    await _scheduler.UnscheduleEvent(eventSpec);
                }
                return true;
            }

            return false;
        }

        #region Testing
        public List<string> LoadedCommandNames()
        {
            List<string> loaded = new List<string>();
            foreach (var processor in _commands.Values)
            {
                loaded.Add(processor.GetType().Name);
            }
            return loaded;
        }
        #endregion 
    }
}
