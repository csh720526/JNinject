using System;

namespace JNinject.Core
{
    public interface IRegistration : IDisposable
    {
        string RegisteNamed { get; }

        Type RegisterType { get; }

        Type AsType { get; }

        IRegistration As<T>();

        IRegistration Named(string tag);

        IRegistration SingleInstance();

        object CreateComponet(IJContext context);
    }
}