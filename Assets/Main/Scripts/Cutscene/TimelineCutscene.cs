using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineCutscene : MonoBehaviour, IPausable
{
    private PlayableDirector _playableDirector;
    [SerializeField] private WordData[] _words;
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

    private void WriteWords(PlayableDirector playableDirector)
    {
        foreach (var word in _words)
            DictionaryManager.WriteWord(word);
    }

    public void Play()
    {
        _playableDirector.time = 0;
        _playableDirector.Evaluate();
        _playableDirector.Play();
    }

    public void Stop()
    {
        _playableDirector.Stop();
    }

    public void Skip()
    {
        var diff = _playableDirector.duration - _playableDirector.time;
        _playableDirector.time += diff;
        _playableDirector.Evaluate();
        _playableDirector.Stop();
    }

    public void Freeze()
    {
        _playableDirector.Pause();
    }

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
