using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform joystickTransform;

    [SerializeField] private float movinTreshold = 0.5f;
    [SerializeField] private int joystickMaxDis = 30;
    [SerializeField] private int dragOffsetDistance = 100;

    public event Action<Vector2> OnMove;

    private void Awake()
    {
        joystickTransform = transform.GetComponent<RectTransform>();

    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickTransform, eventData.position, null, out offset
        );

        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;

        joystickTransform.anchoredPosition = offset * joystickMaxDis;

        Vector2 inputVector = CalculateMovementInput(offset);
        OnMove?.Invoke(inputVector);
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = 0;
        float y = 0;
        
        if (Mathf.Abs(offset.x) > movinTreshold) {
            x = offset.x; 
        }

        if (Mathf.Abs(offset.y) > movinTreshold) {
            y = offset.y;
        }

        return new Vector2(x, y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickTransform.anchoredPosition = Vector2.zero;
        OnMove?.Invoke(Vector2.zero);

    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
