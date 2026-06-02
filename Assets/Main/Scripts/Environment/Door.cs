using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50;
    private float _rotationAngle = 90;
    private Quaternion _startRotation;

    [Header("Axis")]
    [SerializeField] private bool _x;
    [SerializeField] private bool _y = true;
    [SerializeField] private bool _z;

    void Start()
    {
        _startRotation = transform.rotation;
    }

    public void Toggle(bool open)
    {
        StopAllCoroutines();
        StartCoroutine(Rotate(open));
    }

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
