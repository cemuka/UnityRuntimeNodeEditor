using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityRuntimeNodeEditor
{
    public class SocketInput : Socket, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SignalSystem.InvokeInputSocketClick(this, eventData);
        }
    }
}