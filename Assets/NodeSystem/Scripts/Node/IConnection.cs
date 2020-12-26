using System;

public interface IConnection
{
    T GetValue<T>();
    NodeType GetType();
    event Action ValueUpdated;
}