using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
namespace RuntimeNodeEditor
{
    public class LinePointerListener : MonoBehaviour, IPointerClickHandler
    {
        public string connId;
        private SignalSystem _signal;

        public void Init(SignalSystem signal, string connId)
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