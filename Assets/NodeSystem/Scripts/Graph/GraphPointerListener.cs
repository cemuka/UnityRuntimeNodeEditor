using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphPointerListener : MonoBehaviour, IPointerClickHandler
{
    public static event Action<PointerEventData> GraphPointerEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        GraphPointerEvent?.Invoke(eventData);
    }
}
