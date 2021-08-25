using System;

namespace RuntimeNodeEditor
{
    public interface IOutput
    {
        T GetValue<T>();
        event Action ValueUpdated;
    }
}