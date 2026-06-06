using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class RespawningItem : MonoBehaviour
{
    [SerializeField] private GameObject _disappearEffectPrefab;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _interactable;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (_interactable.interactorsSelecting.Count == 0 
            && _rigidbody.linearVelocity.magnitude > 0
            && !QuestManager.Instance.IsInActiveZone(transform))
        {
            Instantiate(_disappearEffectPrefab, transform.position, Quaternion.identity);
            QuestManager.Instance.ReturnToActiveZone(transform);
        }
    }
}
