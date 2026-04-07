using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MultiSocket : MonoBehaviour
{
    [SerializeField] private Transform _attachPoint;
    [SerializeField] private InteractionLayerMask _interactionLayers;
    public UnityEvent<XRGrabInteractable> onSelectEnter;
    public UnityEvent<XRGrabInteractable> onSelectExit;
    private XRGrabInteractable _interactable;

    private void Awake()
    {
        if (!_attachPoint)
            _attachPoint = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        _interactable = other.GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_interactable && _interactable.interactionLayers == _interactionLayers && !_interactable.isSelected)
        {
            onSelectEnter.Invoke(_interactable);
            _interactable.transform.SetParent(transform, false);
            _interactable.transform.SetPositionAndRotation(_attachPoint.localPosition, _attachPoint.localRotation);
            _interactable = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var newInteractable = other.GetComponent<XRGrabInteractable>();
        if (newInteractable && newInteractable.interactionLayers == _interactionLayers && newInteractable.isSelected) { }
            onSelectExit.Invoke(newInteractable);
    }
}
