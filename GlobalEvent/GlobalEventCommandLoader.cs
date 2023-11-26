using GameServerAlpha.Core;
using System;
using System.Collections.Generic;

namespace GameServerAlpha.GlobalEvent
{
    /// <summary>
    /// Automatically load commands on Manager through the reflection
    /// </summary>
    internal class GlobalEventCommandLoader : IAssemblyTypeProcessor
    {
        Dictionary<GlobalEventCommandType, IGlobalEventCommand> _command = new Dictionary<GlobalEventCommandType, IGlobalEventCommand>();
        
        public void ProcessType(Type type)
        {
            if (typeof(IGlobalEventCommand).IsAssignableFrom(type))
            {
                IGlobalEventCommand command = (IGlobalEventCommand)Activator.CreateInstance(type);
                if (!_command.ContainsKey(command.CommandType))
                {
                    _command.Add(command.CommandType, command);
                }
            }
        }

        public void PreProcessType()
        {
        }

        public void PostProcessType()
        {
            GlobalEventManager.Instance.InitCommand(_command);
        }

        
    }
}
