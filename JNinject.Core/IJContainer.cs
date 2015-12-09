using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNinject.Core
{
    public interface IJContainer
    {

        T Resolve<T>();
    }
}
