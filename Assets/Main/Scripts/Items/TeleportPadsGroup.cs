using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TeleportPadsGroup : MonoBehaviour
{
    [SerializeField] private TeleportPad[] _pads;

    public UnityEvent OnPadGroupEnter;
    public UnityEvent OnPadGroupExit;

    private TeleportPad _current;
    private Coroutine _exitCoroutine;

    void Start()
    {
        foreach (var pad in _pads)
        {
            pad.onPadEnter.AddListener(EnterPadsGroup);
            pad.onPadExit.AddListener(ExitPadsGrpup);
        }
    }

    public void ActivateAllPads()
    {
        foreach(var pad in _pads)
        {
            if (pad != _current)
                pad.Activate();
        }
    }

    public void DeactivateAllPads()
    {
        foreach (var pad in _pads)
        {
            if (pad != _current)
                pad.Deactivate();
        }
    }

    private void EnterPadsGroup(TeleportPad pad)
    {
        if (_current == null)
            OnPadGroupEnter.Invoke();
        _current = pad;
    }

    private void ExitPadsGrpup(TeleportPad pad)
    {
        if (_exitCoroutine != null)
            StopCoroutine(_exitCoroutine);

        _exitCoroutine = StartCoroutine(WaitBeforeExit(pad));
    }

    private IEnumerator WaitBeforeExit(TeleportPad pad)
    {
        yield return new WaitForEndOfFrame();
        if(pad == _current)
        {
            OnPadGroupExit.Invoke();
            _current = null;
        }
    }
}
