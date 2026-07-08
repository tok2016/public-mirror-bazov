using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// Manages behaviour of pad for teleportation.
/// </summary>
public class TeleportPad : MonoBehaviour
{
    [SerializeField] private bool _activateOnAwake;
    [SerializeField] private TeleportPad _next;
    [SerializeField] private Material _activated, _deactivated;
    [SerializeField] private Collider _snapVolume;
    [SerializeField] private GameObject _godrays;

    /// <value>
    /// Teleportation destination
    /// </value>
    [field: SerializeField] public TeleportationAnchor Anchor {  get; private set; }

    private MeshRenderer _renderer;
    private bool _isActive;

    public UnityEvent<TeleportPad> onPadEnter;
    public UnityEvent<TeleportPad> onPadExit;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (_activateOnAwake) Activate();
        else Deactivate();
    }

    /// <summary>
    /// Makes pad available for teleportation.
    /// </summary>
    public void Activate()
    {
        _isActive = true;
        _renderer.material = _activated;
        _godrays.SetActive(true);
        _snapVolume.gameObject.SetActive(true);
    }

    /// <summary>
    /// Makes pad unavailable for teleportation.
    /// </summary>
    public void Deactivate()
    {
        _isActive = false;
        _renderer.material = _deactivated;
        _godrays.SetActive(false);
        _snapVolume.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates next pad and disables current to make it unavailable for teleportation repeat.
    /// </summary>
    public void EnterThePad()
    {
        if (_isActive)
        {
            _next?.Activate();
            onPadEnter.Invoke(this);
            _godrays.SetActive(false);
            _snapVolume.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Enables current pad so the player can teleport to it again.
    /// </summary>
    public void ExitThePad()
    {
        if (_isActive)
        {
            onPadExit.Invoke(this);
            _godrays.SetActive(true);
            _snapVolume.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Anchor.teleporting.RemoveAllListeners();
    }
}
