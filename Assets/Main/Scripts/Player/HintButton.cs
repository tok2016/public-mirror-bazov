using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class HintButton : MonoBehaviour
{
    [SerializeField] private InputActionReference _action;
    [SerializeField] private Material _active, _pressed;
    private Material _defaultMaterial;
    private MeshRenderer _renderer;

    private bool _isHintEnabled;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        _defaultMaterial = _renderer?.material;
    }

    private void Update()
    {
        if (_isHintEnabled)
        {
            if (_action.action.WasPressedThisFrame())
                TogglePressHint(true);

            if (_action.action.WasReleasedThisFrame())
                TogglePressHint(false);
        }
    }

    public void ToggleHint(bool enable)
    {
        _isHintEnabled = enable;
        if(_renderer)
            _renderer.material = enable ? _active : _defaultMaterial;
    }

    public void TogglePressHint(bool enable)
    {
        if (_renderer)
            _renderer.material = enable 
                ? _pressed 
                : (_isHintEnabled ? _active : _defaultMaterial);
    }

    public void EnableHint(UIHoverEventArgs args)
    {
        Debug.Log("Enter");
        ToggleHint(true);
    }

    public void DisableHint(UIHoverEventArgs args)
    {
        Debug.Log("Exit");
        ToggleHint(false);
    }

    public void EnableHint(HoverEnterEventArgs args)
    {
        Debug.Log("Enter");
        ToggleHint(true);
    }

    public void DisableHint(HoverExitEventArgs args)
    {
        Debug.Log("Exit");
        ToggleHint(false);
    }
}
