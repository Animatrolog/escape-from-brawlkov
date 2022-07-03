using UnityEngine.EventSystems;

public class OnScreenFloatingJoystick : OnScreenJoystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position, eventData.pressEventCamera);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        background.anchoredPosition = _backgroundStartPosition;
        base.OnPointerUp(eventData);
    }
}