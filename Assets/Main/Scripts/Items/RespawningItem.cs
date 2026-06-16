using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class RespawningItem : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private XRGrabInteractable _interactable;
    private Quaternion _defaultRotation;

    [SerializeField] private GameObject _disappearEffect;
    private ItemActiveZone _exitedZone;
    private int _itemZonesTouching = 0;
    private Coroutine _exitCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
        _defaultRotation = transform.rotation;
    }

    public void RemoveItemZone()
    {
        if (_interactable.isSelected)
            _interactable.selectExited.AddListener(OnItemLetGo);
        else if(_itemZonesTouching == 0)
            ReturnToZone();
    }

    private void OnItemLetGo(SelectExitEventArgs args)
    {
        ReturnToZone();
        _interactable.selectExited.RemoveListener(OnItemLetGo);
    }

    private void ReturnToZone()
    {
        if (!_exitedZone || _itemZonesTouching > 0) return;

        Instantiate(_disappearEffect, transform.position, Quaternion.identity);

        transform.position = _exitedZone.GetRespawnPoint().position;
        transform.rotation = _defaultRotation;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        var itemZone = other.GetComponent<ItemActiveZone>();
        if(itemZone != null)
        {
            _itemZonesTouching++;
            _exitedZone = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var itemZone = other.GetComponent<ItemActiveZone>();
        if(itemZone != null)
        {
            if(_exitCoroutine == null)
                _exitCoroutine = StartCoroutine(WaitBeforeExit(itemZone));
            else
                _itemZonesTouching--;
        }
    }

    private IEnumerator WaitBeforeExit(ItemActiveZone itemZone)
    {
        yield return null;
        _itemZonesTouching--;
        _exitedZone = itemZone;
        RemoveItemZone();
        _exitCoroutine = null;
    }
}
