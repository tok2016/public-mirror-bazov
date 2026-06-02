using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class CollectableItem: MonoBehaviour, ICommentable
{
    [field: SerializeField] public CollectableItemData Data {get; private set;}
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
        //_interactable.selectEntered.AddListener(QuestManager.Instance.OnItemGrab);
        //_interactable.selectExited.AddListener(QuestManager.Instance.OnItemLettingGo);
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
        Interactable.interactionLayers |= (1 << InteractionLayerMask.NameToLayer("Collectable"));
        ToggleGravity(false);
    }

    public void ToggleGravity(bool enableGravity)
    {
        _rigidbody.useGravity = enableGravity;
        _rigidbody.isKinematic = !enableGravity;
    }

    public void WriteWord()
    {
        if (Data.Word && !DictionaryManager.IsWordStored(Data.Word))
            DictionaryManager.WriteWord(Data.Word);
    }

    public void ToggleInteractable(bool enable)
    {
        enabled = enable;
        Interactable.enabled = enable;
    }

    private void OnDisable()
    {
        //_interactable.selectEntered.RemoveAllListeners();
        Interactable.selectExited.RemoveAllListeners();
    }

    public void CommentGrab(string text)
    {
        Debug.Log(text);
    }

    public void CommentLettingGo(string text)
    {
        Debug.Log(text);
    }
}
