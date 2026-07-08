using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Manages dialogue line and timeline as united cutscene. 
/// </summary>
[Serializable]
public class Cutscene
{
    [SerializeField] private DialogueLine _phrase;
    [SerializeField] private TimelineCutscene _timelineCutscene;

    /// <summary>
    /// Plays cutscene content and waits for its end.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Stops cutscene content.
    /// </summary>
    public void Stop()
    {
        if (_phrase)
            DialogueManager.StopLine(_phrase);
        if(_timelineCutscene)
            _timelineCutscene.Stop();
    }

    /// <summary>
    /// Skips all cutscene content.
    /// </summary>
    public void Skip()
    {
        if (_phrase)
            DialogueManager.StopLine(_phrase);
        if(_timelineCutscene)
            _timelineCutscene.Skip();
    }
}
