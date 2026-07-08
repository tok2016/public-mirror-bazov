using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Groups several pads making it as a holistic zone.
/// </summary>
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

    /// <summary>
    /// Activates all pads except current one.
    /// </summary>
    public void ActivateAllPads()
    {
        foreach(var pad in _pads)
        {
            if (pad != _current)
                pad.Activate();
        }
    }

    /// <summary>
    /// Deactivates all pads except current one.
    /// </summary>
    public void DeactivateAllPads()
    {
        foreach (var pad in _pads)
        {
            if (pad != _current)
                pad.Deactivate();
        }
    }

    /// <summary>
    /// Handles a teleportation to the first or last pad in group.
    /// </summary>
    /// <param name="pad">Pad the player has teleported to.</param>
    private void EnterPadsGroup(TeleportPad pad)
    {
        if (_current == null)
            OnPadGroupEnter.Invoke();
        _current = pad;
    }

    /// <summary>
    /// Handles a teleportation from the last or first pad in group.
    /// </summary>
    /// <param name="pad">Pad the player has teleported from.</param>
    private void ExitPadsGrpup(TeleportPad pad)
    {
        if (_exitCoroutine != null)
        {
            StopCoroutine(_exitCoroutine);
            _exitCoroutine = null;
        }

        _exitCoroutine = StartCoroutine(WaitBeforeExit(pad));
    }

    /// <summary>
    /// Exits the group in the next frame.
    /// </summary>
    /// <param name="pad">Pad the player has teleported from.</param>
    /// <returns></returns>
    private IEnumerator WaitBeforeExit(TeleportPad pad)
    {
        yield return null;
        if(pad == _current)
        {
            OnPadGroupExit.Invoke();
            _current = null;
        }

        _exitCoroutine = null;
    }
}
