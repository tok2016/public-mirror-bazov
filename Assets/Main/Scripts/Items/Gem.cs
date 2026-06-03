using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class Gem : MonoBehaviour, ICommentable
{
    [SerializeField] protected CollectableItemData _data;
    [SerializeField] protected float _growthSpeed = 1f;
    protected Rigidbody _rigidbody;
    protected Vector3 _defaultScale;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        while (transform.localScale.magnitude < _defaultScale.magnitude)
        {
            transform.localScale += Vector3.MoveTowards(transform.localScale, _defaultScale, _growthSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = _defaultScale;
    }

    public void CommentGrab(string text)
    {
        Debug.Log(text);
    }

    public void CommentLettingGo(string text)
    {
        Debug.Log(text);
    }

    void ICommentable.WriteWord()
    {
        if (_data.Word)
            DictionaryManager.WriteWord(_data.Word);
    }
}
