using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class CollectableItem: MonoBehaviour
{
    [field: SerializeField] public CollectableItemData Data {get; private set;}
    private Rigidbody _rigidbody;
    private XRGrabInteractable _interactable;

    public bool IsCollected {get; private set;}

    //temporary
    private bool _wasWritten;

    private void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(QuestManager.Instance.OnItemGrab);
        _interactable.selectExited.AddListener(QuestManager.Instance.OnItemLettingGo);
        _interactable.selectExited.AddListener(OnLettingGo);
    }

    public void OnLettingGo(SelectExitEventArgs args)
    {
        ToggleGravity(true);
    }

    public void TransformSocketedItem()
    {
        IsCollected = true;
        gameObject.SetActive(false);
    }

    public void RestoreSocketedItem(Transform attachPoint)
    {
        transform.position = attachPoint.position;
        IsCollected = false;
        gameObject.SetActive(true);
        ToggleGravity(false);
    }

    public void ToggleGravity(bool enableGravity)
    {
        _rigidbody.useGravity = enableGravity;
        _rigidbody.isKinematic = !enableGravity;
    }

    public void CommentWord()
    {
        if (!_wasWritten && Data.Word)
        {
            _wasWritten = true;
            Debug.Log($"Новое слово: {Data.Word.Title} - {Data.Word.Description}");
        }
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveAllListeners();
        _interactable.selectExited.RemoveAllListeners();
    }
}
