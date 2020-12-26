using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrapEventListener : MonoBehaviour, IPointerClickHandler
{
    public static event Action<PointerEventData> GraphMouseEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        GraphMouseEvent?.Invoke(eventData);
    }
}
