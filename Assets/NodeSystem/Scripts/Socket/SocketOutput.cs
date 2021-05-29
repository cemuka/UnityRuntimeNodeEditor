using System;
using UnityEngine.EventSystems;
using UnityEngine;

public class SocketOutput : Socket, IOutput, IDragHandler, IEndDragHandler
{
    private object _value;

    public void SetValue(object value)
    {
        if (_value != value)
        {
            _value = value;
            ValueUpdated?.Invoke();
        }
    }

    public event Action ValueUpdated;

    public T GetValue<T>()
    {
        return(T)_value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SignalSystem.InvokeSocketDragFrom(this);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var item in eventData.hovered)
        {
            var input = item.GetComponent<SocketInput>();
            if (input != null)
            {
                SignalSystem.InvokeInputSocketDropTo(input);
                return;
            }       
        }

        SignalSystem.InvokeInputSocketDropTo(null);
    }
}