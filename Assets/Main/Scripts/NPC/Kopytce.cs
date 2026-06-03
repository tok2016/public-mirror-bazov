using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kopytce : NpcContoller
{
    [SerializeField] private float _minAnimationBreak, _maxAnimationBreak;
    private float _animationBreakTimer;
    private Coroutine _idleAnimationCoroutine;

    private string[] _idleAnimTriggers = { "Eat", "Groom", "Shake", "Stretch" };
    private bool _isVoid = true;

    [Header("Gems generation")]
    [SerializeField] private int _stompsCount = 3;
    [SerializeField] private Gem[] _gems;
    [SerializeField] private float _throwForce = 3;
    [SerializeField] private Transform _throwCenter;
    [SerializeField] private float _throwZoneRadius = 0.25f;
    private Vector3[] _throwOffsets;
    private Stack<Gem> _currentGems;
    private int _gemsPerStomp;

    protected override void Start()
    {
        base.Start();
        _currentGems = new Stack<Gem>();
        _animationBreakTimer = _maxAnimationBreak;

        foreach(var gem in _gems)
            _currentGems.Push(gem);

        if(_stompsCount > _currentGems.Count)
            _stompsCount = _currentGems.Count;

        _gemsPerStomp = (int)Mathf.Ceil((float)_currentGems.Count / _stompsCount);
        _throwOffsets = FormThrowOffsets(_gemsPerStomp);
    }

    protected override void Update()
    {
        base.Update();

        if (_isVoid)
        {
            if (_animationBreakTimer <= 0)
                _idleAnimationCoroutine = StartCoroutine(RandomizeAnimation());
            else
                _animationBreakTimer -= Time.deltaTime;
        }
    }

    private Vector3[] FormThrowOffsets(int positionsCount)
    {
        var throwPositions = new Vector3[positionsCount];
        var angleStep = Mathf.PI * 2 / positionsCount;
        for (int i = 0; i < positionsCount; i++)
        {
            var circlePoint = angleStep * i;
            var x = Mathf.Cos(circlePoint) * _throwZoneRadius;
            var z = Mathf.Sin(circlePoint) * _throwZoneRadius;

            throwPositions[i] = new Vector3(x, 0, z);

            Debug.Log(throwPositions[i]);
        }

        return throwPositions;
    }

    public void ToggleAlert(bool alert)
    {
        StopRandomAnimation();
        _animator.SetBool("IsAlerted", alert);

        if (_idleAnimationCoroutine != null)
            StopCoroutine(_idleAnimationCoroutine);
    }

    public void StartStomping()
    {
        ToggleAlert(true);
        StartCoroutine(Stomp());
    }

    private IEnumerator Stomp()
    {
        for (int i = 0; i < _stompsCount; i++)
        {
            _animator.SetTrigger("Stomp");
            var clipsQueue = _animator.GetCurrentAnimatorClipInfo(0);
            while (clipsQueue.Length > 0 && clipsQueue[0].clip.name != clipsQueue[clipsQueue.Length - 1].clip.name)
                yield return null;

            var currentClip = clipsQueue[0].clip;
            if (currentClip != null)
                yield return new WaitForSeconds(currentClip.length);

            ThrowGems();
        }
    }

    private void ThrowGems()
    {
        var gemsToThrow = Mathf.Clamp(_gemsPerStomp, 0, _currentGems.Count);

        for(int i = 0;  i < gemsToThrow; i++)
        {
            //var direction = GetRandomDirection();
            var gem = _currentGems.Pop();

            var throwOffset = _throwOffsets[i];
            var direction = new Vector3(throwOffset.x, _throwZoneRadius * 2, throwOffset.z);
            Debug.Log(direction);

            gem.transform.position = _throwOffsets[i] + _throwCenter.position;
            gem.Appear();
            gem.Throw(direction.normalized * _throwForce);
        }
    }

    private Vector3 GetRandomDirection()
    {
        var x = Random.Range(-0.5f, 0.5f);
        var y = 0.9f;
        var z = Random.Range(-0.5f, 0.5f);
        return new Vector3(x, y, z);
    }

    public void Bark()
    {
        ToggleAlert(true);
        _animator.SetTrigger("Bark");
    }

    private IEnumerator RandomizeAnimation()
    {
        _isVoid = false;
        var randIndex = Random.Range(0, _idleAnimTriggers.Length);
        _animator.SetTrigger(_idleAnimTriggers[randIndex]);

        var currentClip = _animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        if (currentClip != null)
            yield return new WaitForSeconds(currentClip.length);

        StopRandomAnimation();
    }

    private void StopRandomAnimation()
    {
        _isVoid = true;
        _animationBreakTimer = Mathf.Round(Random.Range(_minAnimationBreak, _maxAnimationBreak));
    }
}
