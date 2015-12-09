using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNinject.Core
{
    public interface IJContainerBuilder
    {
        IRegistration RegisterType<T>();

        IRegistration RegisterType<T>(Func<IJContext, T> func);

        IRegistration Register<T>(T obj);

        IJContainer Build();
    }
}
