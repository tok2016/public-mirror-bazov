using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RespawningItem))]
public class Gem : MonoBehaviour, ICommentable
{
    [SerializeField] protected CollectableItemData _data;
    [SerializeField] protected float _growthSpeed = 0.1f;
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
        var growSpeed = _growthSpeed * Time.deltaTime;
        while (transform.localScale.magnitude < _defaultScale.magnitude)
        {
            transform.localScale += Vector3.MoveTowards(transform.localScale, _defaultScale, growSpeed);
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
