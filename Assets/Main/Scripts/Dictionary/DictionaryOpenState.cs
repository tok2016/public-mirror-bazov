using System.Collections;
using UnityEngine;

/// <summary>
/// Controlls the book when it's opened. Scales the book itself and rotates it front binding.
/// If the book is outside of camera visibility zone, changes its state to grabbed.
/// </summary>
public class DictionaryOpenState : DictionaryState
{
    private float _rotationSpeed = 700;
    private float _scalingSpeed = 10;
    private Vector3 _openedSize;
    private Coroutine _opening, _closing;

    delegate bool Compare(float a, float b);

    public DictionaryOpenState(DictionaryBook book, DictionaryStateMachine stateMachine) : base(book, stateMachine) { }

    /// <summary>
    /// Rotates the front binding, changes book scale and enables canvas.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        _book.EnableCanvas(true);
        _openedSize = _book.DefaultScale * _book.OpenedScale;

        if (_closing != null)
        {
            _book.StopCoroutine(_closing);
            _closing = null;
        }

        _opening = _book.StartCoroutine(OpenFrontBindng());
    }

    /// <summary>
    /// Checks if the book is outside of camera visibility zone.
    /// </summary>
    public override void Update()
    {
        if (!_book.IsInFrontOfCamera())
            _stateMachine.ChangeState(_stateMachine.GrabState);
    }

    /// <summary>
    /// Rotates the front binding back, resets book scale to initial value and disables canvas.
    /// </summary>
    public override void Exit()
    {
        base.Exit();

        if (_opening != null)
        {
            _book.StopCoroutine(_opening);
            _opening = null;
        }

        _closing = _book.StartCoroutine(CloseFrontBinding());
    }

    /// <summary>
    /// Smoothly rotates the front binding of book.
    /// </summary>
    /// <param name="open">If true, rotates front binding towards the opened value (straight angle).</param>
    /// <returns></returns>
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

    /// <summary>
    /// Smoothly scales the book.
    /// </summary>
    /// <param name="open">If true, scales to the opened value.</param>
    /// <returns></returns>
    private IEnumerator RescaleBook(bool open)
    {
        var tagetScale = open ? _openedSize : _book.DefaultScale;
        var targetMagnitude = tagetScale.magnitude;

        Compare compare = open ? IsLess : IsGreater;

        while(compare(_book.ScaleWrapper.localScale.magnitude, targetMagnitude))
        {
            _book.ScaleWrapper.localScale = Vector3.MoveTowards(_book.ScaleWrapper.localScale, tagetScale, _scalingSpeed * Time.deltaTime);
            yield return null;
        }
        _book.ScaleWrapper.localScale = tagetScale;
    }

    /// <summary>
    /// Scales book and rotates its front binding to opened values.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenFrontBindng()
    {
        _book.PlayOpeningSound();
        yield return RescaleBook(true);
        yield return RotateFrontBinding(true);
        _opening = null;
    }

    /// <summary>
    /// Disables canvas, rotates front binding and scales the book to their initial values.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CloseFrontBinding()
    {
        _book.PlayOpeningSound();
        _book.EnableCanvas(false);
        yield return RotateFrontBinding(false);
        if(_stateMachine.Current == _stateMachine.CloseState)
            _book.StartReturing();
        _book.FrontBinding.localRotation = Quaternion.identity;
        yield return RescaleBook(false);
        _book.ScaleWrapper.localScale = _book.DefaultScale;
        _closing = null;
    }
}
