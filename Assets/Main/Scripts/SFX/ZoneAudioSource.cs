using UnityEngine;

/// <summary>
/// Base class for all <c>AudioSource</c> wrappers that change clip zone by zone.
/// </summary>
public abstract class ZoneAudioSource : MonoBehaviour
{
    [SerializeField] protected AudioZone[] _zones;
    [SerializeField] protected AudioClip _defaultClip;

    protected virtual void OnEnable()
    {
        foreach (var zone in _zones)
        {
            zone.OnPlayerEnter += ChangeClip;
            zone.OnPlayerExit += ResetClip;
        }
    }

    /// <summary>
    /// Changes clip.
    /// </summary>
    /// <param name="nextClip">Clip to replace current one with.</param>
    public abstract void ChangeClip(AudioClip nextClip);

    /// <summary>
    /// Chnages clip to default one.
    /// </summary>
    public virtual void ResetClip()
    {
        ChangeClip(_defaultClip);
    }

    protected virtual void OnDisable()
    {
        foreach (var zone in _zones)
        {
            zone.OnPlayerEnter -= ChangeClip;
            zone.OnPlayerExit -= ResetClip;
        }
    }
}
