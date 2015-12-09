using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNinject.Core
{
    public class JContainer : IJContainer, IJContext
    {
        private IEnumerable<IRegistration> registrations;

        public JContainer(IEnumerable<IRegistration> registrations)
        {
            this.registrations = registrations;
        }

        public object Resolve(Type type)
        {
            IEnumerable<IRegistration> registration;

            registration = registrations
                .Where(r => r.AsType.Equals(type));

            if (!registration.Any())
                throw new Exception("Can't find register type");

            return registration.First().CreateComponet(this);
        }

        public T Resolve<T>()
        {
            var registration = registrations
                .Where(r => r.AsType.Equals(typeof(T)))
                .Last();

            return (T)registration.CreateComponet(this);
        }

        public T ResolveName<T>(string tageName)
        {
            var registration = registrations
                .Where(r => r.AsType.Equals(typeof(T)) && r.RegisteNamed.Equals(tageName))
                .Last();

            return (T)registration.CreateComponet(this);
        }
    }
}
