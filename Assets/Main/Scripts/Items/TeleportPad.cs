using UnityEngine;
using UnityEngine.Events;

public class TeleportPad : MonoBehaviour
{
    [SerializeField] private TeleportPad _next;
    [SerializeField] private Material _activated, _deactivated;
    [SerializeField] private Collider _snapVolume;

    private MeshRenderer _renderer;

    public UnityEvent onPadEnter;
    public UnityEvent onPadExit;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        _renderer.material = _deactivated;
        _snapVolume.gameObject.SetActive(false);
    }

    public void Activate()
    {
        _renderer.material = _activated;
        _snapVolume.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _renderer.material = _deactivated;
        _snapVolume.gameObject.SetActive(false);
    }

    public void EnterThePad()
    {
        _next?.Activate();
        onPadEnter.Invoke();
        _snapVolume.gameObject.SetActive(false);
    }

    public void ExitThePad()
    {
        onPadExit.Invoke();
        _snapVolume.gameObject.SetActive(true);
    }
}
