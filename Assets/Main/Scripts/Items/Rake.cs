using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Rake : MonoBehaviour
{
    [SerializeField] private Transform _attachPoint;
    [SerializeField] private InteractionLayerMask _interactionLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Indirect")
        {
            var grabbable = other.GetComponent<XRGrabInteractable>();
            if(grabbable != null && grabbable.interactionLayers == _interactionLayer)
            {
                other.transform.SetParent(_attachPoint);
                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;
                gameObject.SetActive(false);
            }
        }
    }
}
