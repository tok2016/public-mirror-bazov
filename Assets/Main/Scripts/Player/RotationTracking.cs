using UnityEngine;
using UnityEngine.InputSystem;

public class RotationTracking : MonoBehaviour
{
    [SerializeField] private InputActionReference _rotationAction;
    private Vector3 _value;

    [Header("Axis Tracked")]
    [SerializeField] bool x;
    [SerializeField] bool y, z;
    private Vector3 _corrected;

    void Start()
    {
        _corrected = new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
    }

    void Update()
    {
        _value = _rotationAction.action.ReadValue<Quaternion>().eulerAngles;
        transform.rotation = Quaternion.Euler(_value.x * _corrected.x, _value.y * _corrected.y, _value.z * _corrected.z);
    }
}
