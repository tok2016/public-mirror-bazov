using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50;
    private float _rotationAngle = 90;
    private bool _isOpening = false;
    private Quaternion _startRotation;

    void Start()
    {
        _startRotation = transform.rotation;
    }

    public void Toggle()
    {
        StopAllCoroutines();
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        _isOpening = !_isOpening;
        var target = Quaternion.Euler(0, _startRotation.eulerAngles.y - _rotationAngle * (_isOpening ? 1 : 0), 0);
        var speed = _rotationSpeed * Time.deltaTime;

        Debug.Log(Mathf.Abs(transform.rotation.eulerAngles.y - target.eulerAngles.y));

        while (Mathf.Abs(transform.rotation.eulerAngles.y - target.eulerAngles.y) > _rotationAngle * 2 * Time.deltaTime)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed);
            yield return null;
        }

        transform.rotation = target;
    }
}
