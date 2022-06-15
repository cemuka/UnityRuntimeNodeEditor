using UnityEngine;
using UnityEngine.EventSystems;
namespace RuntimeNodeEditor
{
    public class LinePointerListener : MonoBehaviour, IPointerClickHandler
    {
        public string connId;
        private IConnectionEvents _signal;

        public void Init(IConnectionEvents signal, string connId)
        {
            _signal = signal;
            this.connId = connId;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _signal.InvokeNodeConnectionPointerClick(connId, eventData);
        }

    }

}