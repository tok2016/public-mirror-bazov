using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(RespawningItem))]
public class Gem : MonoBehaviour, ICommentable
{
    [SerializeField] protected CollectableItemData _data;
    [SerializeField] protected float _growthSpeed = 0.1f;
    protected Rigidbody _rigidbody;
    protected XRGrabInteractable _interactable;
    protected Vector3 _defaultScale;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
    }

    public void Throw(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void Appear()
    {
        _defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        var growSpeed = _growthSpeed * Time.deltaTime;
        while (transform.localScale.magnitude < _defaultScale.magnitude)
        {
            transform.localScale += Vector3.MoveTowards(transform.localScale, _defaultScale, growSpeed);
            yield return null;
        }

        transform.localScale = _defaultScale;
    }

    public void CommentGrab()
    {
        if(_data.DialogueLine)
            DialogueManager.PlayLine(_data.DialogueLine);
    }

    public void CommentLettingGo()
    {
        
    }

    public void ToggleInteractable(bool enable)
    {
        var collectableLayer = InteractionLayerMask.NameToLayer("Collectable");
        if (enable)
            _interactable.interactionLayers |= (1 << collectableLayer);
        else
            _interactable.interactionLayers &= ~(1 << collectableLayer);
    }
}
