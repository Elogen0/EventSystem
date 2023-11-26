using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerAlpha.Core
{
    public interface IAssemblyTypeProcessor
    {
        void PreProcessType();
        void ProcessType(Type type);
        void PostProcessType();
    }
}
