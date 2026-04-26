using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bag : MonoBehaviour
{
    [SerializeField] private float _removeSpeed = 30;
    [SerializeField] private Transform _attachPoint;
    private XRSocketInteractor[] _sockets;

    private void Awake()
    {
        _sockets = GetComponentsInChildren<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        foreach (var socket in _sockets)
            socket.selectEntered.AddListener(QuestManager.Instance.Check);
    }

    private void Start()
    {
        foreach(var socket in _sockets)
            socket.enabled = false;
    }

    public void Wear(SelectEnterEventArgs args)
    {
        foreach (var socket in _sockets)
            socket.enabled = true;
    }

    public void Remove()
    {
        
    }

    private void OnDisable()
    {
        foreach (var socket in _sockets)
            socket.selectEntered.RemoveAllListeners();
    }
}
