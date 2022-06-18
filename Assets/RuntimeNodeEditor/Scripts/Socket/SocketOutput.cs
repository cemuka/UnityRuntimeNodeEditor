using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace RuntimeNodeEditor
{
    public class SocketOutput : Socket, IOutput, IPointerClickHandler, IDragHandler, IEndDragHandler
    {
        public  Connection  connection;
        private object      _value;

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
            Events.InvokeOutputSocketClick(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Events.InvokeSocketDragFrom(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (var item in eventData.hovered)
            {
                var input = item.GetComponent<SocketInput>();
                if (input != null)
                {
                    Events.InvokeOutputSocketDragDropTo(input);
                    return;
                }
            }

            Events.InvokeOutputSocketDragDropTo(null);
        }
    
        public void Connect(Connection conn)
        {
            connection = conn;
        }

        public void Disconnect()
        {
            connection = null;
        }

        public override bool HasConnection()
        {
            return connection != null;
        }
    }
}