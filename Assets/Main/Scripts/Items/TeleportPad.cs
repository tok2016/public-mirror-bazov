using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportPad : MonoBehaviour
{
    [SerializeField] private TeleportPad _next;
    [SerializeField] private Material _activated, _deactivated;
    [SerializeField] private Collider _snapVolume;
    [SerializeField] private GameObject _godrays;
    [field: SerializeField] public TeleportationAnchor Anchor {  get; private set; }

    private MeshRenderer _renderer;

    public UnityEvent<TeleportPad> onPadEnter;
    public UnityEvent<TeleportPad> onPadExit;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    //private void OnEnable()
    //{
    //    Anchor.teleporting.AddListener(QuestManager.Instance.OnTeleport);
    //}

    void Start()
    {
        Anchor.teleporting.AddListener(QuestManager.Instance.OnTeleport);
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
        onPadEnter.Invoke(this);
        _godrays.SetActive(false);
        _snapVolume.gameObject.SetActive(false);
    }

    public void ExitThePad()
    {
        onPadExit.Invoke(this);
        _godrays.SetActive(true);
        _snapVolume.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Anchor.teleporting.RemoveAllListeners();
    }
}
