using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JNinject.Core
{
    public class Registration : IRegistration, IRegistrationInstance
    {
        private bool disposed;

        private Type registerType;
        private Type asType;
        private object cacheObj;
        private bool isSingleton;
        private string named;
        private Func<IJContext, object> create;
        private ConstructorInfo construct;

        public string RegisteNamed { get { return named; } }

        public Type RegisterType
        {
            private set { registerType = value; }
            get { return registerType; }
        }

        public Type AsType
        {
            private set { asType = value; }
            get { return asType; }
        }

        public Registration(Type type)
            : this(type, null)
        {
        }

        public Registration(Type type, Func<IJContext, object> func)
        {
            registerType = type;
            asType = type;

            if (func != null)
                create = func;
            else
            {
                construct = registerType.GetConstructors()
                    .OrderByDescending(c => c.GetParameters().Count())
                    .First();
            }
        }

        public IRegistration As<T>()
        {
            AsType = typeof(T);
            return this;
        }

        public void SetObject(object obj)
        {
            cacheObj = obj;
        }

        public IRegistration Named(string named)
        {
            this.named = named;

            return this;
        }

        public IRegistration SingleInstance()
        {
            isSingleton = true;

            return this;
        }

        public object CreateComponet(IJContext context)
        {
            if (isSingleton)
            {
                if (cacheObj == null)
                    cacheObj = CreateObject(context);

                return cacheObj;
            }

            return CreateObject(context);
        }

        private object CreateObject(IJContext context)
        {
            if ((registerType.IsValueType || registerType.Equals(typeof(string))) &&
                !string.IsNullOrEmpty(RegisteNamed))
                return cacheObj;

            if (create != null)
                return create.Invoke(context);

            if (construct != null)
            {
                List<object> constructParameters = new List<object>();
                foreach (var item in construct.GetParameters())
                    constructParameters.Add(context.Resolve(item.ParameterType));

                return Activator.CreateInstance(registerType, constructParameters.ToArray());
            }

            throw new Exception("Can't find Create method");
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                create = null;
            }

            disposed = true;
        }
    }
}
