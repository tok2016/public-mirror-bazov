using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Controlls item to be in accessible zone.
/// </summary>
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

    /// <summary>
    /// Removes active zone.
    /// If item has exited the zone but it's still selected by interactor, method waits for its release.
    /// </summary>
    public void RemoveItemZone()
    {
        _itemZonesTouching--;
        if (_interactable.isSelected)
            _interactable.selectExited.AddListener(OnItemLetGo);
        else if(_itemZonesTouching == 0)
            ReturnToZone();
    }

    /// <summary>
    /// Waits for exited item release.
    /// </summary>
    /// <param name="args"></param>
    private void OnItemLetGo(SelectExitEventArgs args)
    {
        ReturnToZone();
        _interactable.selectExited.RemoveListener(OnItemLetGo);
    }


    /// <summary>
    /// Respawns item that has exited all active zones and stops its movement.
    /// </summary>
    private void ReturnToZone()
    {
        if (!_exitedZone || _itemZonesTouching > 0) return;

        Instantiate(_disappearEffect, transform.position, Quaternion.identity);

        transform.position = _exitedZone.GetRespawnPoint().position;
        transform.rotation = _defaultRotation;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Adds active zone when item enters the active zone.
    /// </summary>
    /// <param name="other">Trigger zone that this item has entered.</param>
    private void OnTriggerEnter(Collider other)
    {
        var itemZone = other.GetComponent<ItemActiveZone>();
        if(itemZone != null)
        {
            _itemZonesTouching++;
            _exitedZone = null;
        }
    }

    /// <summary>
    /// Removes active zone when item exits the active zone.
    /// </summary>
    /// <param name="other">Trigger zone that this item has exited.</param>
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

    /// <summary>
    /// Removes active zone in the next frame. 
    /// </summary>
    /// <param name="itemZone">Active zone that item has exited.</param>
    /// <returns></returns>
    private IEnumerator WaitBeforeExit(ItemActiveZone itemZone)
    {
        yield return null;
        _exitedZone = itemZone;
        RemoveItemZone();
        _exitCoroutine = null;
    }

    private void OnDisable()
    {
        _itemZonesTouching = 0;
    }
}
