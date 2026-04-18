using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bag : MonoBehaviour
{
    [SerializeField] private float _removeSpeed = 30;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _interactable;
    private XRSocketInteractor[] _sockets;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
        _sockets = GetComponentsInChildren<XRSocketInteractor>();
    }

    private void Start()
    {
        _interactable.enabled = true;

        foreach(var collider in _interactable.colliders)
            collider.isTrigger = false;

        foreach(var socket in _sockets)
            socket.enabled = false;
    }

    public void Wear(SelectEnterEventArgs args)
    {
        _interactable.enabled = false;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;

        foreach (var collider in _interactable.colliders)
            collider.isTrigger = true;

        foreach (var socket in _sockets)
            socket.enabled = true;
    }

    public void Remove()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        var target = Quaternion.Euler(0, 180, 0);
        var speed = _removeSpeed * Time.deltaTime;

        while (Mathf.Abs(transform.localRotation.eulerAngles.y - target.eulerAngles.y) > 180 * 2 * Time.deltaTime)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, target, speed);
            yield return null;
        }

        transform.localRotation = target;
        gameObject.SetActive(false);
    }
}
