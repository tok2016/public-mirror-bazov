using System.Collections;
using UnityEngine;

/// <summary>
/// Manages grabbable item behaviour specific to gem.
/// </summary>
[RequireComponent(typeof(RespawningItem))]
public class Gem : GrabbableObject
{
    [SerializeField] private float _growthSpeed = 0.1f;
    private Vector3 _defaultScale;

    /// <summary>
    /// Throws gem in a given direction.
    /// </summary>
    /// <param name="force">Vector of the force applied to gem.</param>
    public void Throw(Vector3 force)
    {
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }

    /// <summary>
    /// Activates gem and starts its transformation.
    /// </summary>
    public void Appear()
    {
        _defaultScale = transform.localScale;

        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;

        gameObject.SetActive(true);
        StartCoroutine(Grow());
    }

    /// <summary>
    /// Enlarges gem.
    /// </summary>
    /// <returns></returns>
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
