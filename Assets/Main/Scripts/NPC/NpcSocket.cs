using UnityEngine;
using UnityEngine.Events;

public class NpcSocket : MonoBehaviour
{
    [SerializeField] private Transform _itemAttachPoint;
    public UnityEvent<NpcGrabbable, NpcSocket> onItemEnter;
    public UnityEvent<NpcGrabbable, NpcSocket> onItemExit;

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.GetComponent<NpcGrabbable>();

        if (grabbable && grabbable.enabled)
        {
            grabbable.transform.parent = transform.parent;
            grabbable.transform.localPosition = _itemAttachPoint ? _itemAttachPoint.localPosition : transform.localPosition;
            grabbable.transform.localRotation = Quaternion.identity;

            onItemEnter.Invoke(grabbable, this);
            grabbable.Grab();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var grabbable = other.GetComponent<NpcGrabbable>();

        if (grabbable)
        {
            grabbable.transform.parent = null;

            onItemExit.Invoke(grabbable, this);
            grabbable.LetGo();
        }
    }
}
