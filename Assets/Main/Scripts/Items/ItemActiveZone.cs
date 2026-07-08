using System.Linq;
using UnityEngine;

/// <summary>
/// Check if an item is inside the zone where it must be active and accessible.
/// </summary>
[RequireComponent(typeof(CombinedTrigger))]
public class ItemActiveZone : MonoBehaviour
{
    [SerializeField] private Transform _cameraRespawnPoint, _reservedRespawnPoint;
    private CombinedTrigger _combinedTrigger;

    private void Awake()
    {
        _combinedTrigger = GetComponent<CombinedTrigger>();
        if (!_reservedRespawnPoint)
            _reservedRespawnPoint = transform;
    }

    /// <summary>
    /// Returns point where an item will be respawned at.
    /// </summary>
    /// <returns>
    /// Point in front of camera. 
    /// If it's outside of active zone, method returns specified for this zone point.
    /// </returns>
    public Transform GetRespawnPoint() => _cameraRespawnPoint && IsInActiveZone(_cameraRespawnPoint) 
        ? _cameraRespawnPoint 
        : _reservedRespawnPoint;

    /// <summary>
    /// Checks if an item is inside of active zone. 
    /// </summary>
    /// <param name="item">Item which position needs to be checked.</param>
    /// <returns>Whether an item is inside of active zone.</returns>
    public bool IsInActiveZone(Transform item) => _combinedTrigger
        .Triggers
        .Any(collider => collider.bounds.Contains(item.position));
}
