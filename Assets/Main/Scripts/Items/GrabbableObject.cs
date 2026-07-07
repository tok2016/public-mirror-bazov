using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Base class for all objects able to be grabbed and commented.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable), typeof(PausableRigidbody))]
public abstract class GrabbableObject : MonoBehaviour, ICommentable, IGrabbable
{
    /// <value>
    /// Data about item.
    /// </value>
    [field: SerializeField] public CollectableItemData Data { get; protected set; }
    private XRGrabInteractable _interactable;
    public XRGrabInteractable Interactable => _interactable;

    /// <value>
    /// Rigidbody of item.
    /// </value>
    protected Rigidbody Rigidbody { get; private set; }

    protected virtual void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Toggles item's rigidbody kinematics and gravity.
    /// </summary>
    /// <param name="enable">Whether to enable or disable gravity.</param>
    public virtual void ToggleGravity(bool enable)
    {
        Rigidbody.useGravity = enable;
        Rigidbody.isKinematic = !enable;
    }

    /// <summary>
    /// Changes interaction layer mask to interactable or non-iteractable.
    /// </summary>
    /// <param name="enable">Whether to enable or disable interactivity.</param>
    public virtual void ToggleInteractivity(bool enable)
    {
        var collectableLayer = InteractionLayerMask.NameToLayer("Collectable");
        if (enable)
            Interactable.interactionLayers |= (1 << collectableLayer);
        else
            Interactable.interactionLayers &= ~(1 << collectableLayer);
    }

    /// <summary>
    /// Deactivates item when it's socketed.
    /// </summary>
    public virtual void TransformSocketedItem()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates item and places it to given transform.
    /// </summary>
    /// <param name="attachPoint">Where to place item.</param>
    /// <param name="parent">Where to assign item as a child.</param>
    public virtual void RestoreSocketedItem(Transform attachPoint, Transform parent = null)
    {
        transform.position = attachPoint.position;
        transform.parent = parent;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Comments data about item.
    /// </summary>
    public virtual void CommentGrab()
    {
        if (Data.DialogueLine)
            DialogueManager.PlayLine(Data.DialogueLine);
    }

    public virtual void CommentLettingGo()
    {

    }
}
