using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HintButtonState
{
    Default = 0,
    Active = 1,
    UI = 2,
    Pressed = 3,
    Disabled = 4
}

public class HintButton : HintObject
{
    [SerializeField] private InputActionReference _action;
    [SerializeField] private HintButtionProps _hintButtionProps;
    [SerializeField] private GameObject _uiHint;

    private HintButtonState _state, _prevState;

    protected override void Awake()
    {
        base.Awake();
        _state = HintButtonState.Default;
    }

    protected override void Start()
    {
        base.Start();
        ToggleMaterial(false);
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
            _renderer.material = _hintButtionProps.StateMaterials[_state];

            _uiHint.SetActive(enable);
        }
    }

    public void ToggleUIHint(bool enable)
    {
        if (_renderer)
        {
            if(enable && _state != HintButtonState.UI)
                _prevState = _state;

            _state = enable ? HintButtonState.UI : _prevState;
            _renderer.material = _hintButtionProps.StateMaterials[_state];
            _uiHint.SetActive(enable);
        }
    }

    public void ToggleWarningHint(bool enable)
    {
        if(_renderer && _state < HintButtonState.UI)
        {
            _state = enable ? HintButtonState.Active : HintButtonState.Default;
            _renderer.material = _hintButtionProps.StateMaterials[_state];
        }
    }

    public void TogglePressHint(bool enable)
    {
        if (_renderer && (_state > HintButtonState.Default || _state < HintButtonState.Disabled))
        {
            if (enable && _state != HintButtonState.Pressed)
                _prevState = _state;
            _state = enable ? HintButtonState.Pressed : _prevState;
            _renderer.material = _hintButtionProps.StateMaterials[_state];
        }
    }

    public void ToggleDisable(bool enable)
    {
        if (_renderer && _state > HintButtonState.UI)
        {
            _state = enable ? HintButtonState.Disabled : HintButtonState.Active;
            _renderer.material = _hintButtionProps.StateMaterials[_state];
        }   
    }
}
