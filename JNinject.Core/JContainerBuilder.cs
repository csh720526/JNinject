using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNinject.Core
{
    public class JContainerBuilder : IJContainerBuilder
    {
        private List<IRegistration> registrations;

        public JContainerBuilder()
        {
            registrations = new List<IRegistration>();
        }

        public IRegistration RegisterType<T>()
        {
            IRegistration reg = new Registration(typeof(T));

            registrations.Add(reg);

            return reg;
        }

        public IRegistration RegisterType<T>(Func<IJContext, T> func)
        {
            IRegistration reg = new Registration(typeof(T), context => func.Invoke(context));

            registrations.Add(reg);

            return reg;
        }

        public IRegistration Register<T>(T obj)
        {
            IRegistration reg = new Registration(typeof(T));

            (reg as IRegistrationInstance).SetObject(obj);

            registrations.Add(reg);

            return reg;
        }

        public IJContainer Build()
        {
            IJContainer container = new JContainer(registrations);
            
            return container;
        }
    }
}
