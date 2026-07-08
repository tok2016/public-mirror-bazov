using UnityEngine;

/// <summary>
/// Manages behaviour of NPC common for every of them.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class NpcContoller : MonoBehaviour, IPausable
{
    [Header("NPC Base")]
    [SerializeField] protected Character _characterName;
    [SerializeField] protected Animator _animator;

    /// <value>
    /// Audio source used to make sounds from NPC.
    /// </value>
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

    /// <summary>
    /// Plays given sound.
    /// </summary>
    /// <param name="sound">Sound to play.</param>
    public virtual void PlaySound(AudioClip sound)
    {
        _currentLine = null;
        NpcAudioSource.Stop();
        NpcAudioSource.clip = sound;
        NpcAudioSource.Play();
    }

    /// <summary>
    /// Plays given dialogue line.
    /// </summary>
    /// <remarks>
    /// If the line had lower priority than currently playing one, it's ignored.
    /// </remarks>
    /// <param name="line">Dialogue line to play.</param>
    public virtual void PlayLine(DialogueLine line)
    {
        if (NpcAudioSource.isPlaying && _currentLine && line.Priority < _currentLine.Priority)
            return;

        NpcAudioSource.Stop();
        NpcAudioSource.clip = line.Clip;
        _currentLine = line;
        NpcAudioSource.Play();
    }

    /// <summary>
    /// Stops playing given line if it's currently beeing played.
    /// </summary>
    /// <param name="line">Dialogue line to stop.</param>
    public virtual void StopLine(DialogueLine line)
    {
        if (_currentLine == line)
            StopLine();
    }

    /// <summary>
    /// Stops every sound or line.
    /// </summary>
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

    /// <summary>
    /// Pauses playing sound or line.
    /// </summary>
    public virtual void Freeze()
    {
        _animator.speed = 0;
        NpcAudioSource.Pause();
    }

    /// <summary>
    /// Continues playing sound or line.
    /// </summary>
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
