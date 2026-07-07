using System.Collections;
using UnityEngine;

/// <summary>
/// Represent the door objects. Manages their rotation around the selected axes.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50;
    private float _rotationAngle = 90;
    private Quaternion _startRotation;
    private AudioSource _audioSource;

    [Header("Axis")]
    [SerializeField] private bool _x;
    [SerializeField] private bool _y = true;
    [SerializeField] private bool _z;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _startRotation = transform.rotation;
    }

    /// <summary>
    /// Opens (rotates to defined angle) or closes (return to initial rotation) the door.
    /// </summary>
    /// <param name="open">Whether to open or close the door.</param>
    public void Toggle(bool open)
    {
        StopAllCoroutines();
        _audioSource.Play();
        StartCoroutine(Rotate(open));
    }

    /// <summary>
    /// Smoothly rotates the door to target angle.
    /// </summary>
    /// <param name="open">If true, the target is the defined angle or initial one otherwise.</param>
    /// <returns></returns>
    private IEnumerator Rotate(bool open)
    {
        var angleDiff = _rotationAngle * (open ? 1 : 0);
        var angle = _startRotation.eulerAngles - new Vector3(_x ? angleDiff : 0, _y ? angleDiff : 0, _z ? angleDiff : 0);
        var target = Quaternion.Euler(angle);

        while (Mathf.Abs(transform.rotation.eulerAngles.y - target.eulerAngles.y) > _rotationAngle * 2 * Time.deltaTime)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = target;
    }
}
