using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NpcContoller : MonoBehaviour
{
    [Header("NPC Base")]
    [SerializeField] protected Character _characterName;
    [SerializeField] protected Animator _animator;
    protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        DialogueManager.AddCharacter(_characterName, this);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual void Pronounce(AudioClip sound)
    {
        _audioSource.Stop();
        _audioSource.clip = sound;
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        DialogueManager.RemoveCharacter(_characterName);
    }
}
