using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class RespawningItem : MonoBehaviour
{
    [SerializeField] private GameObject _disappearEffectPrefab;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _interactable;
    private Quaternion _defaultRotation;
    private float _minSpeedToStop = 0.2f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
        _defaultRotation = transform.rotation;
    }

    void Update()
    {
        if (_interactable.interactorsSelecting.Count == 0 
            && _rigidbody.linearVelocity.magnitude > _minSpeedToStop
            && !QuestManager.IsInActiveZone(transform))
        {
            Instantiate(_disappearEffectPrefab, transform.position, Quaternion.identity);
            QuestManager.ReturnToActiveZone(transform);

            transform.rotation = _defaultRotation;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}
