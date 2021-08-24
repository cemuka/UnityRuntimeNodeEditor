using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace UnityRuntimeNodeEditor
{
    public class SocketOutput : Socket, IOutput, IPointerClickHandler, IDragHandler, IEndDragHandler
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
            return (T)_value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SignalSystem.InvokeOutputSocketClick(this, eventData);
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
                    SignalSystem.InvokeOutputSocketDragDropTo(input);
                    return;
                }
            }

            SignalSystem.InvokeOutputSocketDragDropTo(null);
        }
    }
}