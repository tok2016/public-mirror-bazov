using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(RespawningItem))]
public class CollectableItem: MonoBehaviour, ICommentable
{
    [field: SerializeField] public CollectableItemData Data {get; protected set;}
    public XRGrabInteractable Interactable { get; private set; }
    private Rigidbody _rigidbody;

    public UnityEvent OnCollceted, OnRestored;

    public bool IsCollected {get; private set;}

    private void Awake()
    {
        Interactable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Interactable.selectExited.AddListener(OnLettingGo);
    }

    public void OnLettingGo(SelectExitEventArgs args)
    {
        ToggleGravity(true);
    }

    public void TransformSocketedItem()
    {
        IsCollected = true;
        gameObject.SetActive(false);
        OnCollceted.Invoke();
    }

    public void RestoreSocketedItem(Transform attachPoint)
    {
        transform.position = attachPoint.position;
        IsCollected = false;
        gameObject.SetActive(true);
        OnRestored.Invoke();
        ToggleInteractable(true);
        ToggleGravity(false);
    }

    public void ToggleGravity(bool enableGravity)
    {
        _rigidbody.useGravity = enableGravity;
        _rigidbody.isKinematic = !enableGravity;
    }

    private void OnDisable()
    {
        Interactable.selectExited.RemoveAllListeners();
    }

    public void CommentGrab()
    {
        if(Data.DialogueLine)
            DialogueManager.PlayLine(Data.DialogueLine);
    }

    public void CommentLettingGo()
    {
        
    }

    public void ToggleInteractable(bool enable)
    {
        var collectableLayer = InteractionLayerMask.NameToLayer("Collectable");
        if (enable)
            Interactable.interactionLayers |= (1 << collectableLayer);
        else
            Interactable.interactionLayers &= ~(1 << collectableLayer);
    }
}
