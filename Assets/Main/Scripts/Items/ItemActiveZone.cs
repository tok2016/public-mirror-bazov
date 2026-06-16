using System.Linq;
using UnityEngine;

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

    public Transform GetRespawnPoint() => _cameraRespawnPoint && IsInActiveZone(_cameraRespawnPoint) 
        ? _cameraRespawnPoint 
        : _reservedRespawnPoint;

    public bool IsInActiveZone(Transform item) => _combinedTrigger
        .Triggers
        .Any(collider => collider.bounds.Contains(item.position));
}
