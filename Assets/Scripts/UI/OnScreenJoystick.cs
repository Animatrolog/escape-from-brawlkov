using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;


/// <summary>
/// A stick control displayed on screen and moved around by touch or other pointer
/// input.
/// </summary>

public class OnScreenJoystick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;

    protected Vector2 _backgroundStartPosition;
    private RectTransform baseRect = null;

    protected virtual void Start()
    {
        _backgroundStartPosition = background.anchoredPosition;
        baseRect = GetComponent<RectTransform>();
        m_StartPos = ((RectTransform)handle.transform).anchoredPosition;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle.transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle.transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - m_PointerDownPos;

        HandleInput(delta / movementRange, movementRange);

        delta = Vector2.ClampMagnitude(delta, movementRange);
        ((RectTransform)handle.transform).anchoredPosition = m_StartPos + (Vector3)delta;

        SendValueToControl(delta / movementRange);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ((RectTransform)handle.transform).anchoredPosition = m_StartPos;
        SendValueToControl(Vector2.zero);
    }


    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    [FormerlySerializedAs("movementRange")]
    [SerializeField]
    private float m_MovementRange = 50;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    private Vector3 m_StartPos;
    private Vector2 m_PointerDownPos;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition, Camera cam)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

    protected virtual void HandleInput(Vector2 input, float radius) { }

}

