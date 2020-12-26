using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform _canvasRectTransform;
    private RectTransform _panelRectTransform;
    private Vector2 _pointerOffset;
    private Vector2 _localPointerPos;

    public void Init()
    {
        _panelRectTransform = this.transform.parent.GetComponent<RectTransform>();
        _canvasRectTransform =  this.transform.parent.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_panelRectTransform, eventData.position, 
                                                                eventData.pressEventCamera, out _pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 pointerPos = ClampToWindow(eventData);
            var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, pointerPos, 
                                                                            eventData.pressEventCamera, out _localPointerPos);
            if (success)
            {
                _panelRectTransform.localPosition = _localPointerPos - _pointerOffset;
            }
        }
    }

    private Vector2 ClampToWindow(PointerEventData eventData)
    {
        var rawPointerPos = eventData.position;
        var canvasCorners = new Vector3[4];
        _canvasRectTransform.GetWorldCorners(canvasCorners);

        var clampedX = Mathf.Clamp(rawPointerPos.x, canvasCorners[0].x, canvasCorners[2].x);
        var clampedY = Mathf.Clamp(rawPointerPos.y, canvasCorners[0].y, canvasCorners[2].y);

        var newPointerPos = new Vector2(clampedX, clampedY);
        return newPointerPos;
    }
}
