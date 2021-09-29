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

        public void OnPointerClick(PointerEventData eventData)
        {
            SignalSystem.InvokeNodeConnectionPointerClick(connId, eventData);
        }

    }

}