using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RespawningItem))]
public class Gem : GrabbableObject
{
    [SerializeField] private float _growthSpeed = 0.1f;
    private Vector3 _defaultScale;

    public void Throw(Vector3 force)
    {
        Rigidbody.AddForce(force, ForceMode.Impulse);
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
}
