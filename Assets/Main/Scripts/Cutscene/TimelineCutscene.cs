using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Wraps up <c>PlayableDirector</c> with dictionary words mention and pause logic.
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class TimelineCutscene : MonoBehaviour, IPausable
{
    private PlayableDirector _playableDirector;
    [SerializeField] private WordData[] _words;

    /// <value>
    /// State of PlayableDirector.
    /// </value>
    public PlayState State => _playableDirector.state;

    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        if (_playableDirector.playOnAwake)
        {
            _playableDirector.playOnAwake = false;
            _playableDirector.Stop();
        }
    }

    private void OnEnable()
    {
        _playableDirector.stopped += WriteWords;
        Pause.onPause += Freeze;
        Pause.onContinue += Unfreeze;
    }

    /// <summary>
    /// Writes words mentioned in timeline.
    /// </summary>
    /// <param name="playableDirector"></param>
    private void WriteWords(PlayableDirector playableDirector)
    {
        foreach (var word in _words)
            DictionaryManager.WriteWord(word);
    }

    /// <summary>
    /// Starts timeline over.
    /// </summary>
    public void Play()
    {
        _playableDirector.time = 0;
        _playableDirector.Evaluate();
        _playableDirector.Play();
    }

    /// <summary>
    /// Stops timeline.
    /// </summary>
    public void Stop()
    {
        _playableDirector.Stop();
    }

    /// <summary>
    /// Skips timeline.
    /// </summary>
    public void Skip()
    {
        var diff = _playableDirector.duration - _playableDirector.time;
        _playableDirector.time += diff;
        _playableDirector.Evaluate();
        _playableDirector.Stop();
    }

    /// <summary>
    /// Pauses timeline.
    /// </summary>
    public void Freeze()
    {
        _playableDirector.Pause();
    }

    /// <summary>
    /// Resumes timeline.
    /// </summary>
    public void Unfreeze()
    {
        _playableDirector.Resume();
    }

    private void OnDisable()
    {
        _playableDirector.stopped -= WriteWords;
        Pause.onPause -= Freeze;
        Pause.onContinue -= Unfreeze;
    }
}
