using System;

namespace JNinject.Core
{
    public interface IJContext
    {
        object Resolve(Type type);

        T Resolve<T>();

        T ResolveName<T>(string tageName);
    }
}