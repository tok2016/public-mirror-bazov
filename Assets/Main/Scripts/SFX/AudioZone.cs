using System;
using UnityEngine;

/// <summary>
/// Handles player entrance and exit to turn on / off the sound specific to this area. 
/// </summary>
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

    /// <summary>
    /// Handles player's entrance to the zone.
    /// </summary>
    /// <param name="other">Collider that entered the zone.</param>
    protected void OnZoneEnter(Collider other)
    {
        if (other.tag == "Player")
            OnPlayerEnter?.Invoke(_clip);
    }

    /// <summary>
    /// Handles player's exit from the zone.
    /// </summary>
    /// <param name="other">Collider that exited the zone.</param>
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
