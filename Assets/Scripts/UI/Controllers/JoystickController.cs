using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    [SerializeField] private RectTransform _centerArea;
    [SerializeField] private RectTransform _handle;
    [InputControl(layout = nameof(Vector2))]
    [SerializeField] private string _stickControlPath;
    [SerializeField] private float _movementRange = 100f;
    [SerializeField] bool _hideOnPointerUp;
    [SerializeField] bool _centralizeOnPointerUp;
    [SerializeField] VirtualJoystickType _joystickType;

    private Canvas _canvas;
    private RectTransform _baseRect = null;
    private OnScreenStick _handleStickController = null;
    private CanvasGroup _bgCanvasGroup = null;
    private Vector2 _initialPosition = Vector2.zero;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _baseRect = GetComponent<RectTransform>();
        _bgCanvasGroup = _centerArea.GetComponent<CanvasGroup>();
        _handleStickController = _handle.gameObject.AddComponent<OnScreenStick>();
        _handleStickController.movementRange = _movementRange;
        _handleStickController.controlPath = _stickControlPath;

        Vector2 center = new(0.5f, 0.5f);
        _centerArea.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;

        _initialPosition = _centerArea.anchoredPosition;

        if (_joystickType == VirtualJoystickType.Fixed)
        {
            _centerArea.anchoredPosition = _initialPosition;
            _bgCanvasGroup.alpha = 1;
        }
        else if (_joystickType == VirtualJoystickType.Floating)
        {
            if (_hideOnPointerUp) _bgCanvasGroup.alpha = 0;
            else _bgCanvasGroup.alpha = 1;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerEventData constructedEventData = new(EventSystem.current)
        {
            position = _handle.position
        };
        _handleStickController.OnPointerDown(constructedEventData);

        if (_joystickType == VirtualJoystickType.Floating)
        {
            _centerArea.anchoredPosition = GetAnchoredPosition(eventData.position);

            if (_hideOnPointerUp)
                _bgCanvasGroup.alpha = 1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_joystickType == VirtualJoystickType.Floating)
        {
            _handleStickController.OnDrag(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_joystickType == VirtualJoystickType.Floating)
        {
            if (_centralizeOnPointerUp)
                _centerArea.anchoredPosition = _initialPosition;

            if (_hideOnPointerUp) _bgCanvasGroup.alpha = 0;
            else _bgCanvasGroup.alpha = 1;
        }

        PointerEventData constructedEventData = new(EventSystem.current)
        {
            position = Vector2.zero
        };

        _handleStickController.OnPointerUp(constructedEventData);
    }

    private Vector2 GetAnchoredPosition(Vector2 screenPosition)
    {
        Camera cam = (_canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, cam, out Vector2 localPoint))
        {
            Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
            return localPoint - (_centerArea.anchorMax * _baseRect.sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }

}
