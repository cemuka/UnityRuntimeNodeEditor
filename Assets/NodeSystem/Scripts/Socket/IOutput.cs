using System;

namespace UnityRuntimeNodeEditor
{
    public interface IOutput
    {
        T GetValue<T>();
        event Action ValueUpdated;
    }
}