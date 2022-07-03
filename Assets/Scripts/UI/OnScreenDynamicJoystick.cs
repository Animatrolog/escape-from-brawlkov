using UnityEngine;
using UnityEngine.EventSystems;

public class OnScreenDynamicJoystick : OnScreenJoystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
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

    protected override void HandleInput(Vector2 input, float radius)
    {
        if (input.magnitude > moveThreshold)
        {
            Vector2 difference = input.normalized * (input.magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
    }
}
