using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace RuntimeNodeEditor
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
            Signal.InvokeOutputSocketClick(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Signal.InvokeSocketDragFrom(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (var item in eventData.hovered)
            {
                var input = item.GetComponent<SocketInput>();
                if (input != null)
                {
                    Signal.InvokeOutputSocketDragDropTo(input);
                    return;
                }
            }

            Signal.InvokeOutputSocketDragDropTo(null);
        }
    }
}