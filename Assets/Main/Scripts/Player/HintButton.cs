using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum HintButtonState
{
    Default = 0,
    Disabled = 1,
    Active = 2,
    Pressed = 3
}

public class HintButton : HintObject
{
    [SerializeField] private InputActionReference _action;
    [SerializeField] private Material _pressedMaterial, _disabledMaterial;

    private HintButtonState _state;
    private Dictionary<HintButtonState, Material> _materials;

    protected override void Start()
    {
        base.Start();
        _state = HintButtonState.Default;
        _defaultMaterial = _renderer?.material;
        _materials = new Dictionary<HintButtonState, Material>() {
            {HintButtonState.Default, _defaultMaterial },
            {HintButtonState.Disabled, _disabledMaterial},
            {HintButtonState.Active, _hintMaterial},
            {HintButtonState.Pressed, _pressedMaterial}
        };
    }

    private void Update()
    {
        if (_state == HintButtonState.Active || _state == HintButtonState.Pressed)
        {
            if (_action.action.WasPressedThisFrame())
                TogglePressHint(true);

            if (_action.action.WasReleasedThisFrame())
                TogglePressHint(false);
        }
    }

    public override void ToggleMaterial(bool enable)
    {
        if(_renderer)
        {
            _state = enable ? HintButtonState.Active : HintButtonState.Default;
            _renderer.material = _materials[_state];
        }
    }

    public void TogglePressHint(bool enable)
    {
        if (_renderer && (_state == HintButtonState.Active || _state == HintButtonState.Pressed))
        {
            _state = enable ? HintButtonState.Pressed : HintButtonState.Active;
            _renderer.material = _materials[_state];
        }
    }

    public void ToggleDisable(bool enable)
    {
        if (_renderer && _state != HintButtonState.Default)
        {
            _state = enable ? HintButtonState.Disabled : HintButtonState.Active;
            _renderer.material = _materials[_state];
        }   
    }
}
