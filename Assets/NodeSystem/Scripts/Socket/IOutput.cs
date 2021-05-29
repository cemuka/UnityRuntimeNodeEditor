using System;

public interface IOutput
{
    T GetValue<T>();
    event Action ValueUpdated;
}