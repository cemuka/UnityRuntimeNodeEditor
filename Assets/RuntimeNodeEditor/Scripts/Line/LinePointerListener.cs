using UnityEngine;
using UnityEngine.EventSystems;
namespace RuntimeNodeEditor
{
    public class LinePointerListener : MonoBehaviour, IPointerClickHandler
    {
        public string connId;
        private IConnectionEvents _connectionEvents;

        public void Init(IConnectionEvents connectionEvents, string connId)
        {
            _connectionEvents = connectionEvents;
            this.connId = connId;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _connectionEvents.InvokeConnectionPointerClick(connId, eventData);
        }

    }

}