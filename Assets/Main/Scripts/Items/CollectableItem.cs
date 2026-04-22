using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class CollectableItem: MonoBehaviour
{
    [field: SerializeField] public CollectableItemData Data {get; private set;}
    [SerializeField] private InteractionLayerMask _socketedLayers;
    private XRGrabInteractable _interactable;
    private float _commentaryTimeOffset = 0.8f;

    public bool IsCollected {get; private set;}

    //temporary
    private bool _wasWritten;

    private void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnSelect);
        _interactable.selectEntered.AddListener(QuestManager.Instance.OnItemGrab);
        _interactable.selectExited.AddListener(onLettingGo);
    }

    public void OnSelect(SelectEnterEventArgs args)
    {
        if (args.interactorObject.GetType() != typeof(XRSocketInteractor))
            Debug.Log(Data.Commentary);
        StopAllCoroutines();

        if(!_wasWritten && Data.Word)
        {
            _wasWritten = true;
            Debug.Log($"Новое слово: {Data.Word.Title} - {Data.Word.Description}");
        }
    }

    private void onLettingGo(SelectExitEventArgs args)
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(CommentLettingGo(args));
    }

    private IEnumerator CommentLettingGo(SelectExitEventArgs args)
    {
        yield return new WaitForSeconds(_commentaryTimeOffset);
        QuestManager.Instance.OnItemLettingGo(args);
    }

    public void TransformSocketedItem()
    {
        foreach (var col in _interactable.colliders)
            col.transform.localScale /= 4;

        _interactable.interactionLayers = _socketedLayers;
        IsCollected = true;
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveAllListeners();
        _interactable.selectExited.RemoveAllListeners();
    }
}
