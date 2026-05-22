using UnityEngine;
using UnityEngine.Events;

public class TeleportPad : MonoBehaviour
{
    [SerializeField] private TeleportPad _next;
    [SerializeField] private Material _activated, _deactivated;
    [SerializeField] private Collider _snapVolume;
    [SerializeField] private GameObject _godrays;

    private MeshRenderer _renderer;

    public UnityEvent onPadEnter;
    public UnityEvent onPadExit;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        _renderer.material = _activated;
        _godrays.SetActive(true);
        _snapVolume.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _renderer.material = _deactivated;
        _godrays.SetActive(false);
        _snapVolume.gameObject.SetActive(false);
    }

    public void EnterThePad()
    {
        _next?.Activate();
        onPadEnter.Invoke();
        _godrays.SetActive(false);
        _snapVolume.gameObject.SetActive(false);
    }

    public void ExitThePad()
    {
        onPadExit.Invoke();
        _godrays.SetActive(true);
        _snapVolume.gameObject.SetActive(true);
    }
}
