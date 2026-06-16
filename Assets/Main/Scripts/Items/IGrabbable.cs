using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public interface IGrabbable
{
    public XRGrabInteractable Interactable { get; }
    public void ToggleInteractivity(bool enable);
    public void ToggleGravity(bool enable);
    public void TransformSocketedItem();
    public void RestoreSocketedItem(Transform attachPoint, Transform parent);
}
