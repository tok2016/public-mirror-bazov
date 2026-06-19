using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NpcContoller : MonoBehaviour
{
    [Header("NPC Base")]
    [SerializeField] protected Character _characterName;
    [SerializeField] protected Animator _animator;
    public AudioSource AudioSource { get; private set; }
    protected DialogueLine _currentLine;

    protected virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.loop = false;
        DialogueManager.AddCharacter(_characterName, this);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual void PlaySound(AudioClip sound)
    {
        AudioSource.Stop();
        AudioSource.clip = sound;
        AudioSource.Play();
    }

    public virtual void PlayLine(DialogueLine line)
    {
        if (AudioSource.isPlaying && line.Priority < _currentLine.Priority)
            return;

        AudioSource.Stop();
        AudioSource.clip = line.Clip;
        AudioSource.Play();
        _currentLine = line;
    }

    public virtual void ShutUp()
    {
        AudioSource.Stop();
    }

    private void OnDestroy()
    {
        DialogueManager.RemoveCharacter(_characterName);
    }
}
