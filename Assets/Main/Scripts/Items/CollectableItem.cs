using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(RespawningItem))]
public class CollectableItem: GrabbableObject
{
    public bool IsCollected {get; private set;}

    private void OnEnable()
    {
        Interactable.selectExited.AddListener(OnLettingGo);
    }

    public void OnLettingGo(SelectExitEventArgs args)
    {
        ToggleGravity(true);
    }

    public override void TransformSocketedItem()
    {
        base.TransformSocketedItem();
        IsCollected = true;
    }

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
