using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.Core
{
    public class AssemblyTypeLoader
    {
        private List<IAssemblyTypeProcessor> _processors = new List<IAssemblyTypeProcessor>();

        public void Load()
        {
            var types = Assembly.GetCallingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (typeof(IAssemblyTypeProcessor).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    IAssemblyTypeProcessor instance = (IAssemblyTypeProcessor)Activator.CreateInstance(type);
                    _processors.Add(instance);
                }
            }

            foreach (var processor in _processors)
            {
                processor.PreProcessType();
            }

            foreach (Type type in types)
            {
                if (!type.IsInterface && !type.IsAbstract)
                {
                    foreach (var processor in _processors)
                    {
                        processor.ProcessType(type);
                    }
                }
            }

            foreach (var processor in _processors)
            {
                processor.PostProcessType();
            }
        }
    }
}
