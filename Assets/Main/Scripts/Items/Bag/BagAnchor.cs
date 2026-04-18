using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BagAnchor : MonoBehaviour
{
    [SerializeField] private InputActionReference _rotationAction;
    private float _yRotationAngle;
    private XRSocketInteractor _interactor;

    private void Awake()
    {
        _interactor = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        _interactor.selectEntered.AddListener(AttachBag);
    }

    private void Update()
    {
        _yRotationAngle = _rotationAction.action.ReadValue<Quaternion>().eulerAngles.y;
        transform.localRotation = Quaternion.Euler(0, _yRotationAngle, 0);
    }

    public void AttachBag(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.SetParent(transform, false);
        args.interactableObject.transform.position = _interactor.attachTransform.position;
        args.interactableObject.transform.rotation = _interactor.attachTransform.rotation;
        _interactor.enabled = false;
    }

    private void OnDisable()
    {
        _interactor.selectEntered.RemoveListener(AttachBag);
    }
}
