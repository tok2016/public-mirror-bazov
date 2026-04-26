using UnityEngine;
using UnityEngine.Events;

public class NpcGrabbableAttach : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.GetComponent<NpcGrabbable>();

        if(grabbable && grabbable.IsForPlayer && grabbable.IsGrabbed)
        {
            grabbable.Attach();
        }
    }
}
