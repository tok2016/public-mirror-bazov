using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ItemActiveZone : MonoBehaviour
{
    [SerializeField] private Transform _cameraRespawnPoint, _reservedRespawnPoint;
    [SerializeField] private CombinedTrigger _combinedTrigger;
    [SerializeField] private XRGrabInteractable[] _importantInteractables;

    private void Awake()
    {
        if (!_reservedRespawnPoint)
            _reservedRespawnPoint = transform;
    }

    private void OnEnable()
    {
        _combinedTrigger.OnTriggerGroupExit += OnActiveZoneExit;
    }

    private void Start()
    {
        ToggleActive(false);
    }

    public void ToggleActive(bool enable)
    {
        if (enable)
        {
            foreach (var item in _importantInteractables)
                if(!IsInActiveZone(item.transform))
                    ReturnExitedItem(item);
        }

        gameObject.SetActive(enable);
    }

    private void OnItemLetGo(SelectExitEventArgs args)
    {
        ReturnExitedItem(args.interactableObject);
        args.interactableObject.selectExited.RemoveListener(OnItemLetGo);
    }

    private void OnActiveZoneExit(Collider other)
    {
        var interactable = other.GetComponent<IXRSelectInteractable>();
        if(interactable != null)
        {
            if (interactable.interactorsSelecting.Count == 0)
                ReturnExitedItem(interactable);
            else
                interactable.selectExited.AddListener(OnItemLetGo);
        }
    }

    private void ReturnExitedItem(IXRSelectInteractable interactable)
    {
        var respawnPoint = _cameraRespawnPoint && IsInActiveZone(_cameraRespawnPoint) 
            ? _cameraRespawnPoint
            : _reservedRespawnPoint;

        interactable.transform.position = respawnPoint.position;
        interactable.transform.localRotation = Quaternion.identity;

        var rigidbody = interactable.transform.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private bool IsInActiveZone(Transform item) => _combinedTrigger
        .Triggers
        .Any(collider => collider.bounds.Contains(item.position));

    private void OnDisable()
    {
        _combinedTrigger.OnTriggerGroupExit -= OnActiveZoneExit;
    }
}
