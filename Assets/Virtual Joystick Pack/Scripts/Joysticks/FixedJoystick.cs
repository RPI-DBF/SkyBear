using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    Vector2 joystickPosition = Vector2.zero;
    private Camera cam = new Camera();

    void Start()
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = direction / (background.sizeDelta.x / 2);
        inputVector = new Vector2(Mathf.Clamp(inputVector.x, -1, 1), Mathf.Clamp(inputVector.y, -1, 1));
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}