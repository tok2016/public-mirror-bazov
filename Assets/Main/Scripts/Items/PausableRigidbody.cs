using UnityEngine;

/// <summary>
/// Freezes object's movement ruled by rigidbody.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PausableRigidbody : MonoBehaviour, IPausable
{
    private Rigidbody _rigidbody;

    private Vector3 _linearVelocity, _angularVelocity;
    private bool _wasKinematic;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Pause.onPause += Freeze;
        Pause.onContinue += Unfreeze;
    }

    /// <summary>
    /// Saves rigidbody's current velocity and makes it kinematic.
    /// </summary>
    public void Freeze()
    {
        _linearVelocity = _rigidbody.linearVelocity;
        _angularVelocity = _rigidbody.angularVelocity;
        _wasKinematic = _rigidbody.isKinematic;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    /// <summary>
    /// Returns rigidbody values before freeze.
    /// </summary>
    public void Unfreeze()
    {
        _rigidbody.linearVelocity = _linearVelocity;
        _rigidbody.angularVelocity = _angularVelocity;
        _rigidbody.isKinematic = _wasKinematic;
    }

    private void OnDisable()
    {
        Pause.onPause -= Freeze;
        Pause.onContinue -= Unfreeze;
    }
}
