using System;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    public Action<AudioClip> OnPlayerEnter;
    public Action OnPlayerExit;

    [SerializeField] protected CombinedTrigger _combinedTrigger;

    private void OnEnable()
    {
        _combinedTrigger.OnTriggerGroupEnter += OnZoneEnter;
        _combinedTrigger.OnTriggerGroupExit += OnZoneExit;
    }

    protected void OnZoneEnter(Collider other)
    {
        if (other.tag == "Player")
            OnPlayerEnter?.Invoke(_clip);
    }

    protected void OnZoneExit(Collider other)
    {
        if(other.tag == "Player")
            OnPlayerExit?.Invoke();
    }

    private void OnDisable()
    {
        _combinedTrigger.OnTriggerGroupEnter -= OnZoneEnter;
        _combinedTrigger.OnTriggerGroupExit -= OnZoneExit;
    }
}
