using UnityEngine;
using UnityEngine.InputSystem;

public class MetaButtonMovement : MonoBehaviour
{
    [Header("Grip")]
    [SerializeField] private Transform _gripButton;
    [SerializeField] private Vector2 _gripXPositionBoundaries;
    [SerializeField] private InputActionReference _gripAction;

    [Header("Trigger")]
    [SerializeField] private Transform _triggerButton;
    [SerializeField] private Vector2 _triggerXRotationBoundaries;
    [SerializeField] protected InputActionReference _triggerAction;

    [Header("Stick")]
    [SerializeField] private Transform _stick;
    [SerializeField] private Vector2 _stickXZTLeanAngle;
    private Vector3 _defaultStickRotation;
    [SerializeField] public InputActionReference _stickAction;

    [Header("A/X")]
    [SerializeField] private Transform _aXButton;
    [SerializeField] private Vector2 _aXPositionYBoundaries;
    [SerializeField] private InputActionReference _aXAction;

    [Header("B/Y")]
    [SerializeField] private Transform _bYButton;
    [SerializeField] private Vector2 _bYPositionYBoundaries;
    [SerializeField] private InputActionReference _bYAction;

    [Header("Home/Menu")]
    [SerializeField] private Transform _homeMenuButton;
    [SerializeField] private Vector2 _homeMenuPositionYBoundaries;
    [SerializeField] private InputActionReference _homeMenuAction;

    private void Start()
    {
        _defaultStickRotation = _stick.localRotation.eulerAngles;
    }

    void Update()
    {
        MoveGripButton();
        MoveTriggerButton();
        MoveStick();

        MovePadButton(_aXButton, _aXAction, _aXPositionYBoundaries);
        MovePadButton(_bYButton, _bYAction, _bYPositionYBoundaries);
        MovePadButton(_homeMenuButton, _homeMenuAction, _homeMenuPositionYBoundaries);
    }

    private void MoveGripButton()
    {
        if (_gripAction.action.IsPressed() || _gripAction.action.WasReleasedThisFrame())
        {
            var gripVal = _gripAction.action.ReadValue<float>();
            _gripButton.localPosition = new Vector3(
                Mathf.Lerp(_gripXPositionBoundaries.x, _gripXPositionBoundaries.y, gripVal),
                _gripButton.localPosition.y,
                _gripButton.localPosition.z
            );
        }
    }

    private void MoveTriggerButton()
    {
        if(_triggerAction.action.IsPressed() || _triggerAction.action.WasReleasedThisFrame())
        {
            var triggerVal = _triggerAction.action.ReadValue<float>();
            _triggerButton.localRotation = Quaternion.Euler(
                Mathf.Lerp(_triggerXRotationBoundaries.x, _triggerXRotationBoundaries.y, triggerVal),
                _triggerButton.localRotation.eulerAngles.y,
                _triggerButton.localRotation.eulerAngles.z
            );
        }
    }

    private void MoveStick()
    {
        if (_stickAction.action.IsPressed() || _stickAction.action.WasReleasedThisFrame())
        {
            var stickVal = _stickAction.action.ReadValue<Vector2>();

            _stick.localRotation = Quaternion.Euler(
                stickVal.y * _stickXZTLeanAngle.x + _defaultStickRotation.x, 
                _stick.localRotation.eulerAngles.y,
                stickVal.x * _stickXZTLeanAngle.y + _defaultStickRotation.z
            );
        }
    }

    private void MovePadButton(Transform button, InputActionReference actionRef, Vector2 yPosBoundaries)
    {
        if(actionRef.action.IsPressed() || actionRef.action.WasReleasedThisFrame())
        {
            var actionValue = actionRef.action.ReadValue<float>();
            button.localPosition = new Vector3(
                button.localPosition.x,
                Mathf.Lerp(yPosBoundaries.x, yPosBoundaries.y, actionValue),
                button.localPosition.z
            );
        }
    }
}
