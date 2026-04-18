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

    //temporary
    private bool _wasWritten;

    private void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(onSelect);
        _interactable.selectExited.AddListener(onLettingGo);
    }

    private void onSelect(SelectEnterEventArgs args)
    {
        if (args.interactorObject.GetType() != typeof(XRSocketInteractor))
            Debug.Log(Data.Commentary);
        StopAllCoroutines();

        if(!_wasWritten)
        {
            _wasWritten = true;
            Debug.Log($"Новое слово: {Data.Word.Title} - {Data.Word.Description}");
        }
    }

    private void onLettingGo(SelectExitEventArgs args)
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(CommentLettingGo());
    }

    private IEnumerator CommentLettingGo()
    {
        yield return new WaitForSeconds(_commentaryTimeOffset);
        Debug.Log("Положи в суму, так надёжнее будет");
    }

    public void TransformSocketedItem()
    {
        foreach (var col in _interactable.colliders)
            col.transform.localScale /= 4;

        _interactable.interactionLayers = _socketedLayers;
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(onSelect);
        _interactable.selectExited.RemoveListener(onLettingGo);
    }
}
