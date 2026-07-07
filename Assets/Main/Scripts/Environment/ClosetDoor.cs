using UnityEngine;

/// <summary>
/// Plays effects of closet door rotation.
/// </summary>
[RequireComponent(typeof(RandomizedAudioSource))]
public class ClosetDoor : MonoBehaviour
{
    [SerializeField] private float _soundThreshold;
    private RandomizedAudioSource _audioSource;
    private HingeJoint _joint;
    private bool _wasMoving, _isMoving;

    private void Awake()
    {
        _audioSource = GetComponent<RandomizedAudioSource>();
        _joint = GetComponent<HingeJoint>();
    }

    void Update()
    {
        _isMoving = Mathf.Abs(_joint.velocity) > _soundThreshold;
        if(_isMoving && !_wasMoving && !_audioSource.IsPlaying)
            _audioSource.Play();

        _wasMoving = _isMoving;
    }
}
