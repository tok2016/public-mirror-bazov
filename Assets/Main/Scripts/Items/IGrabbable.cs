using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Declares behaviour for grabbables.
/// </summary>
public interface IGrabbable
{
    /// <value>
    /// Instance of <c>XRGrabInteractable</c> connected to object.
    /// </value>
    public XRGrabInteractable Interactable { get; }

    /// <summary>
    /// Enables or disables object interactivity.
    /// </summary>
    /// <param name="enable">Whether to enable or disable interactivity.</param>
    public void ToggleInteractivity(bool enable);

    /// <summary>
    /// Enables or disables gravity put on the object.
    /// </summary>
    /// <param name="enable">Whether to enable or disable gravity.</param>
    public void ToggleGravity(bool enable);

    /// <summary>
    /// Transforms object when it's socketed.
    /// </summary>
    public void TransformSocketedItem();

    /// <summary>
    /// Restores item object was socketed.
    /// </summary>
    /// <param name="attachPoint">Where to place object.</param>
    /// <param name="parent">Where to assign object as a child.</param>
    public void RestoreSocketedItem(Transform attachPoint, Transform parent);
}
