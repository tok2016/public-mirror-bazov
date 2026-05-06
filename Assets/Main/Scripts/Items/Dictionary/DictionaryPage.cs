using System.Collections;
using UnityEngine;

public class DictionaryPage : MonoBehaviour
{
    [field: SerializeField] public DictionaryPageSide Front {  get; private set; }
    [field: SerializeField] public DictionaryPageSide Back { get; private set; }
    private float _rotationSpeed = 500f;
    private bool _isFront = true;

    void Start()
    {
        Front.gameObject.SetActive(true);
        Back.gameObject.SetActive(false);
        transform.localRotation = Quaternion.identity;
    }

    public void Turn()
    {
        StartCoroutine(Rotate());
    }

    public void Close()
    {
        transform.SetAsLastSibling();
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator Rotate()
    {
        transform.SetAsLastSibling();
        _isFront = !_isFront;
        var target = _isFront ? 1 : 179;
        var speed = _rotationSpeed * Time.deltaTime;

        while (Mathf.Abs(transform.localRotation.eulerAngles.y - target) > 0.5f)
        {
            var angleDiff = transform.localRotation.eulerAngles.y - 90;
            if (angleDiff < 0 && !Front.gameObject.activeInHierarchy 
                || angleDiff > 0 && Front.gameObject.activeInHierarchy)
            {
                Front.gameObject.SetActive(_isFront);
                Back.gameObject.SetActive(!_isFront);
            }

            var to = Mathf.MoveTowardsAngle(transform.localRotation.eulerAngles.y, target, speed);
            transform.localRotation = Quaternion.Euler(0, to, 0);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, target, 0);
    }
}
