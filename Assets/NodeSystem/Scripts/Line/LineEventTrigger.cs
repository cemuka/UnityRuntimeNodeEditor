using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
public  class LineEventTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler
{
    public string ID;
    public BezierCurveDrawer drawer;
    public void OnPointerClick(PointerEventData eventData)
    {
        SignalSystem.InvokeLineClick(ID, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SignalSystem.InvokeLineDown(ID, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SignalSystem.InvokeLineDrag(ID, eventData);
    }

}

