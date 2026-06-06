using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemActiveZone : MonoBehaviour
{
    [SerializeField] private Transform _cameraRespawnPoint, _reservedRespawnPoint;
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        if (!_reservedRespawnPoint)
            _reservedRespawnPoint = transform;
    }

    public bool IsItemInActiveZone(Transform item)
    {
        var localObjPoint = transform.InverseTransformPoint(item.position);
        localObjPoint -= _collider.center;
        var halfSize = _collider.size * 0.5f;

        bool insideX = Mathf.Abs(localObjPoint.x) <= halfSize.x;
        bool insideY = Mathf.Abs(localObjPoint.y) <= halfSize.y;
        bool insideZ = Mathf.Abs(localObjPoint.z) <= halfSize.z;

        return insideX && insideY && insideZ;
    }

    public virtual void ReturnToActiveZone(Transform item)
    {
        item.position = _cameraRespawnPoint && IsItemInActiveZone(_cameraRespawnPoint) 
            ? _cameraRespawnPoint.position 
            : _reservedRespawnPoint.position;
    }
}
