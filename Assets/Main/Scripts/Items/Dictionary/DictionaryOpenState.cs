using System.Collections;
using UnityEngine;

public class DictionaryOpenState : DictionaryState
{
    private float _rotationSpeed = 700;
    private float _scalingSpeed = 10;
    private Vector3 _openedSize = new Vector3(0.75f, 0.75f, 0.75f);
    private Coroutine _opening, _closing;

    delegate bool Compare(float a, float b);

    public DictionaryOpenState(DictionaryBook book, DictionaryStateMachine stateMachine) : base(book, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _book.EnableCanvas(true);

        if(_closing != null)
            _book.StopCoroutine(_closing);

        _opening = _book.StartCoroutine(OpenFrontBindng());
    }

    public override void Update()
    {
        if (!_book.IsInFrontOfCamera())
            _stateMachine.ChangeState(_stateMachine.GrabState);
    }

    public override void Exit()
    {
        base.Exit();

        if(_opening != null)
            _book.StopCoroutine(_opening);

        _closing = _book.StartCoroutine(CloseFrontBinding());
    }

    private IEnumerator RotateFrontBinding(bool open)
    {
        var targetAngle = open ? 179 : 0;
        while (Mathf.Abs(_book.FrontBinding.localRotation.eulerAngles.y - targetAngle) > 0.5f)
        {
            var to = Mathf.MoveTowardsAngle(_book.FrontBinding.localRotation.eulerAngles.y, targetAngle, _rotationSpeed * Time.deltaTime);
            _book.FrontBinding.localRotation = Quaternion.Euler(0, to, 0);
            yield return null;
        }

        _book.FrontBinding.localRotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private bool IsLess(float a, float b) => a < b;
    private bool IsGreater(float a, float b) => a > b;

    private IEnumerator RescaleBook(bool enlarge)
    {
        var tagetScale = enlarge ? _openedSize : _book.DefaultScale;
        var targetMagnitude = tagetScale.magnitude;

        Compare compare = enlarge ? IsLess : IsGreater;

        while(compare(_book.ScaleWrapper.localScale.magnitude, targetMagnitude))
        {
            _book.ScaleWrapper.localScale = Vector3.MoveTowards(_book.ScaleWrapper.localScale, tagetScale, _scalingSpeed * Time.deltaTime);
            yield return null;
        }
        _book.ScaleWrapper.localScale = tagetScale;
    }

    private IEnumerator OpenFrontBindng()
    {
        yield return RescaleBook(true);
        yield return RotateFrontBinding(true);
    }

    private IEnumerator CloseFrontBinding()
    {
        _book.EnableCanvas(false);
        yield return RotateFrontBinding(false);
        if(_stateMachine.Current == _stateMachine.CloseState)
            _book.StartReturing();
        _book.FrontBinding.localRotation = Quaternion.identity;
        yield return RescaleBook(false);
        _book.ScaleWrapper.localScale = _book.DefaultScale;
    }
}
