using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manages grabbable item that can be collected.
/// </summary>
[RequireComponent(typeof(RespawningItem))]
public class CollectableItem: GrabbableObject
{
    /// <value>
    /// Whether item was collected or not.
    /// </value>
    public bool IsCollected {get; private set;}

    private void OnEnable()
    {
        Interactable.selectExited.AddListener(OnLettingGo);
    }

    /// <summary>
    /// Enables gravity when item was released.
    /// </summary>
    /// <param name="args"></param>
    public void OnLettingGo(SelectExitEventArgs args)
    {
        ToggleGravity(true);
    }

    public override void TransformSocketedItem()
    {
        base.TransformSocketedItem();
        IsCollected = true;
    }

    /// <summary>
    /// Activates item, places it to given transform and disables its gravity so it can be accessible to player.
    /// </summary>
    /// <param name="attachPoint">Where to place item.</param>
    /// <param name="parent">Where to assign item as a child.</param>
    public override void RestoreSocketedItem(Transform attachPoint, Transform parent = null)
    {
        base.RestoreSocketedItem(attachPoint, parent);
        IsCollected = false;

        ToggleInteractivity(true);
        ToggleGravity(false);
    }

    private void OnDisable()
    {
        Interactable.selectExited.RemoveListener(OnLettingGo);
    }    
}
