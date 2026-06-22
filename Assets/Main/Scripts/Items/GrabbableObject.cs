using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable), typeof(PausableRigidbody))]
public abstract class GrabbableObject : MonoBehaviour, ICommentable, IGrabbable
{
    [field: SerializeField] public CollectableItemData Data { get; protected set; }
    private XRGrabInteractable _interactable;
    public XRGrabInteractable Interactable => _interactable;
    protected Rigidbody Rigidbody { get; private set; }

    protected virtual void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void ToggleGravity(bool enable)
    {
        Rigidbody.useGravity = enable;
        Rigidbody.isKinematic = !enable;
    }

    public virtual void ToggleInteractivity(bool enable)
    {
        var collectableLayer = InteractionLayerMask.NameToLayer("Collectable");
        if (enable)
            Interactable.interactionLayers |= (1 << collectableLayer);
        else
            Interactable.interactionLayers &= ~(1 << collectableLayer);
    }

    public virtual void TransformSocketedItem()
    {
        gameObject.SetActive(false);
    }

    public virtual void RestoreSocketedItem(Transform attachPoint, Transform parent = null)
    {
        transform.position = attachPoint.position;
        transform.parent = parent;
        gameObject.SetActive(true);
    }

    public virtual void CommentGrab()
    {
        if (Data.DialogueLine)
            DialogueManager.PlayLine(Data.DialogueLine);
    }

    public virtual void CommentLettingGo()
    {

    }
}
