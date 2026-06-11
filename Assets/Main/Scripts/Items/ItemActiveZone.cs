using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
public class ItemActiveZone : MonoBehaviour
{
    [SerializeField] private Transform _cameraRespawnPoint, _reservedRespawnPoint;
    private BoxCollider[] _colliders;

    private void Awake()
    {
        _colliders = GetComponents<BoxCollider>();
        if (!_reservedRespawnPoint)
            _reservedRespawnPoint = transform;
    }

    private bool IsItemInCollider(BoxCollider collider, Transform item)
    {
        var localObjPoint = transform.InverseTransformPoint(item.position);
        localObjPoint -= collider.center;
        var halfSize = collider.size * 0.5f;

        bool insideX = Mathf.Abs(localObjPoint.x) <= halfSize.x;
        bool insideY = Mathf.Abs(localObjPoint.y) <= halfSize.y;
        bool insideZ = Mathf.Abs(localObjPoint.z) <= halfSize.z;

        return insideX && insideY && insideZ;
    }

    public bool IsItemInActiveZone(Transform item) => _colliders.Any(collider => IsItemInCollider(collider, item));

    public virtual void ReturnToActiveZone(Transform item)
    {
        item.position = _cameraRespawnPoint && IsItemInActiveZone(_cameraRespawnPoint) 
            ? _cameraRespawnPoint.position 
            : _reservedRespawnPoint.position;
    }
}
