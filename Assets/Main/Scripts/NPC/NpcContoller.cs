using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NpcContoller : MonoBehaviour, IPausable
{
    [Header("NPC Base")]
    [SerializeField] protected Character _characterName;
    [SerializeField] protected Animator _animator;
    public AudioSource NpcAudioSource { get; private set; }
    protected DialogueLine _currentLine;

    protected virtual void Awake()
    {
        NpcAudioSource = GetComponent<AudioSource>();
        NpcAudioSource.loop = false;
        DialogueManager.AddCharacter(_characterName, this);
    }

    protected virtual void OnEnable()
    {
        Pause.onPause += Freeze;
        Pause.onContinue += Unfreeze;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual void PlaySound(AudioClip sound)
    {
        NpcAudioSource.Stop();
        NpcAudioSource.clip = sound;
        NpcAudioSource.Play();
    }

    public virtual void PlayLine(DialogueLine line)
    {
        if (NpcAudioSource.isPlaying && _currentLine && line.Priority < _currentLine.Priority)
            return;

        NpcAudioSource.Stop();
        NpcAudioSource.clip = line.Clip;
        _currentLine = line;
        NpcAudioSource.Play();
    }

    public virtual void StopLine(DialogueLine line)
    {
        if (_currentLine == line)
            StopLine();
    }

    public virtual void StopLine()
    {
        _currentLine = null;
        NpcAudioSource.Stop();
        NpcAudioSource.clip = null;
    }

    private void OnDestroy()
    {
        DialogueManager.RemoveCharacter(_characterName);
    }

    public virtual void Freeze()
    {
        _animator.speed = 0;
        NpcAudioSource.Pause();
    }

    public virtual void Unfreeze()
    {
        _animator.speed = 1;
        NpcAudioSource.UnPause();
    }

    protected virtual void OnDisable()
    {
        Pause.onPause -= Freeze;
        Pause.onContinue -= Unfreeze;
    }
}
