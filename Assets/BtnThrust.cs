using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnThrust : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isPressed = false;

    public bool IsPressed()
    {
        return isPressed;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
