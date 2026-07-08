using System.Collections;
using UnityEngine;

/// <summary>
/// Represents Kopytce character. Manages gems appearance.
/// </summary>
public class Kopytce : NpcContoller
{
    [Header("Animation")]
    [SerializeField] private AnimationClip _stompClip;
    [SerializeField] private float _minAnimationBreak, _maxAnimationBreak;
    private float _animationBreakTimer;
    private Coroutine _idleAnimationCoroutine;

    private string[] _idleAnimTriggers = { "Groom", "Shake", "Stretch" };
    private bool _isVoid = true;

    [Header("Gems generation")]
    [SerializeField] private int _stompsCount = 3;
    [SerializeField] private Gem[] _gems;
    [SerializeField] private float _throwForce = 3;
    [SerializeField] private Transform _throwCenter;
    [SerializeField] private float _throwZoneRadius = 0.25f;
    [SerializeField] private GameObject _throwEffect;
    [SerializeField] private AudioClip _stompSound;
    private Vector3[] _throwOffsets;
    private int _currentGem = 0;
    private int _gemsPerStomp;

    protected override void Start()
    {
        base.Start();
        ShuffleGems();

        _animationBreakTimer = _maxAnimationBreak;
        _stompsCount = Mathf.Clamp(_stompsCount, 0, _gems.Length);

        _gemsPerStomp = (int)Mathf.Ceil((float)_gems.Length / _stompsCount);
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

    /// <summary>
    /// Generates gems throw positions relative to the hoof.
    /// </summary>
    /// <param name="positionsCount">Amount of positions to generate for one stomp.</param>
    /// <returns>Array of throw positions for one stomp.</returns>
    private Vector3[] FormThrowOffsets(int positionsCount)
    {
        var throwPositions = new Vector3[positionsCount];
        var angleStep = Mathf.PI * 2 / positionsCount;
        for (int i = 0; i < positionsCount; i++)
            throwPositions[i] = GetThrowOffset(i == 0 ? Vector3.zero : throwPositions[i - 1], angleStep);

        return throwPositions;
    }

    /// <summary>
    /// Calculates next position relative to given point with given angle shift.
    /// </summary>
    /// <param name="origin">Center position that the new position will be calculated relative to.</param>
    /// <param name="angleStep">Shift in degrees.</param>
    /// <returns>Gem throw position.</returns>
    private Vector3 GetThrowOffset(Vector3 origin, float angleStep)
    {
        var angle = Mathf.Atan2(origin.z, origin.x);
        var x = Mathf.Cos(angleStep + angle) * _throwZoneRadius;
        var z = Mathf.Sin(angleStep + angle) * _throwZoneRadius;

        return new Vector3(x, 0, z);
    }

    /// <summary>
    /// Enables or disables alert animation.
    /// </summary>
    /// <param name="alert">If true, enables alert animation.</param>
    public void ToggleAlert(bool alert)
    {
        StopRandomAnimation();
        _animator.SetBool("IsAlerted", alert);

        if (_idleAnimationCoroutine != null)
        {
            StopCoroutine(_idleAnimationCoroutine);
            _idleAnimationCoroutine = null;
        }
    }

    /// <summary>
    /// Alerts NPC and starts gems generation and throwing.
    /// </summary>
    public void StartStomping()
    {
        ToggleAlert(true);
        StartCoroutine(Stomp());
    }

    /// <summary>
    /// Triggers stomp animation and throw some gems.
    /// </summary>
    /// <remarks>
    /// If random idle animation is playing, waits for it's end.
    /// </remarks>
    /// <returns></returns>
    private IEnumerator Stomp()
    {
        for (int i = 0; i < _stompsCount; i++)
        {
            _animator.SetTrigger("Stomp");

            while (_animator.GetCurrentAnimatorClipInfo(0).Length > 0 
                && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != _stompClip.name)
                yield return null;

            var currentClip = _animator.GetCurrentAnimatorClipInfo(0)[0];
            if (currentClip.clip != null)
                yield return new WaitForSeconds(currentClip.clip.length);

            PlaySound(_stompSound);
            _throwEffect.SetActive(true);
            ThrowGems();
        }

        ToggleAlert(false);
        _currentGem = 0;
    }

    /// <summary>
    /// Throw gems in one stomp. After every throwing shifts positions by calculated angle.
    /// </summary>
    private void ThrowGems()
    {
        var angleOffset = Mathf.PI * 2 / (_gemsPerStomp * _stompsCount);
        var gemsToThrow = Mathf.Clamp(_gemsPerStomp, 0, _gems.Length - _currentGem);

        for(int i = 0; i < gemsToThrow; i++)
        {
            var gem = _gems[i + _currentGem];
            var throwOffset = _throwOffsets[i];
            var direction = new Vector3(throwOffset.x, _throwZoneRadius * 3, throwOffset.z);

            gem.transform.position = _throwOffsets[i] + _throwCenter.position;
            gem.Appear();
            gem.Throw(direction.normalized * _throwForce);

            _throwOffsets[i] = GetThrowOffset(_throwOffsets[i], angleOffset);
        }
        _currentGem += gemsToThrow;
    }

    /// <summary>
    /// Triggers bark animation.
    /// </summary>
    public void Bark()
    {
        ToggleAlert(true);
        _animator.SetTrigger("Bark");
    }

    /// <summary>
    /// Randomly chooses and triggers one of idle animation clips.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomizeAnimation()
    {
        _isVoid = false;
        var randIndex = Random.Range(0, _idleAnimTriggers.Length);
        _animator.SetTrigger(_idleAnimTriggers[randIndex]);

        var currentClip = _animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        if (currentClip != null)
            yield return new WaitForSeconds(currentClip.length);

        StopRandomAnimation();
        _idleAnimationCoroutine = null;
    }

    /// <summary>
    /// Stops idle animation and calculates time between animation triggers.
    /// </summary>
    private void StopRandomAnimation()
    {
        _isVoid = true;
        _animationBreakTimer = Mathf.Round(Random.Range(_minAnimationBreak, _maxAnimationBreak));
    }

    /// <summary>
    /// Randomly shuffles gems collection.
    /// </summary>
    private void ShuffleGems()
    {
        for (int i = _gems.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (_gems[i], _gems[randomIndex]) = (_gems[randomIndex], _gems[i]);
        }
    }
}
