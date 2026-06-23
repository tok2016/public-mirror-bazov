using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class Cutscene
{
    [SerializeField] private DialogueLine _phrase;
    [SerializeField] private TimelineCutscene _timelineCutscene;

    public IEnumerator Play()
    {
        while(Pause.IsPaused)
            yield return null;

        if(_phrase)
        {
            var source = DialogueManager.PlayLine(_phrase);
            yield return new WaitWhile(() => Pause.IsPaused || source.isPlaying);
            DialogueManager.StopLine(_phrase);
        }

        if (_timelineCutscene)
        {
            _timelineCutscene.Play();
            yield return new WaitWhile(() => Pause.IsPaused || _timelineCutscene.State == PlayState.Playing);
        }
    }

    public void Stop()
    {
        if (_phrase)
            DialogueManager.StopLine(_phrase);
        if(_timelineCutscene)
            _timelineCutscene.Stop();
    }

    public void Skip()
    {
        if (_phrase)
            DialogueManager.StopLine(_phrase);
        if(_timelineCutscene)
            _timelineCutscene.Skip();
    }
}
