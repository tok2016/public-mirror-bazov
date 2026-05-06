using System.Collections;
using UnityEngine;

public class DictionaryOpenState : DictionaryState
{
    private float _rotationSpeed = 500;
    private float _appearanceDistance = 2f;

    public DictionaryOpenState(DictionaryBook book) : base(book) { }

    public override void Enter(Transform parent)
    {
        base.Enter(parent);

        var position = _book.transform.position.normalized * _appearanceDistance;
        Debug.Log(position);
        _book.transform.position = position;

        _book.Resize(Vector3.one);
        _book.EnableCanvas(true);
        _book.StartCoroutine(RotateFrontBinding(true));
    }

    public override void Update()
    {
        _book.transform.LookAt(_book.Camera.position);
    }

    public override void Exit()
    {
        base.Exit();
        _book.StopAllCoroutines();
        _book.EnableCanvas(false);
        _book.FrontBinding.localRotation = Quaternion.identity;
        _book.Resize(_book.DefaultScale);
    }

    private IEnumerator RotateFrontBinding(bool open)
    {
        var target = open ? 179 : 0;
        while(Mathf.Abs(_book.FrontBinding.localRotation.eulerAngles.y - target) > 0.5f)
        {
            var to = Mathf.MoveTowardsAngle(_book.FrontBinding.localRotation.eulerAngles.y, target, _rotationSpeed * Time.deltaTime);
            _book.FrontBinding.localRotation = Quaternion.Euler(0, to, 0);
            yield return null;
        }

        _book.FrontBinding.localRotation = Quaternion.Euler(0, target, 0);
    }
}
