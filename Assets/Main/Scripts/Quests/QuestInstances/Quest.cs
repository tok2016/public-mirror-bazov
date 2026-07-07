using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// Base class for all quests. Manages quest life cycle common for all quests.
/// </summary>
public abstract class Quest : MonoBehaviour
{
    /// <value>
    /// Data about quest.
    /// </value>
    [Header("Main")]
    [field: SerializeField] public QuestData Data { get; private set; }

    /// <value>
    /// Quest to unlock after the completion of this one.
    /// </value>
    [field: SerializeField] public Quest Next {  get; private set; }
    [SerializeField] protected Cutscene[] _startCutscenes, _endCutscenes;

    private bool _skipCutscene = false;
    private Cutscene _currentCutscene;

    /// <value>
    /// Current state of quest life cycle 
    /// </value>
    public QuestState State { get; protected set; } = QuestState.Locked;

    [Header("Enter")]
    public UnityEvent onActivate;
    public UnityEvent onReturn;
    public UnityEvent onFirstEnter;
    public event Action<Quest> onQuestLocked, onQuestStart;

    [Header("Exit")]
    public UnityEvent onExit;
    public Action<Quest> onQuestStop;

    [Header("Complete")]
    public UnityEvent onDeactivate;
    public UnityEvent onComplete;
    public Action<Quest> onQuestEnd;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    /// <summary>
    /// Makes quest available to enter.
    /// </summary>
    public void Unlock()
    {
        State = QuestState.Available;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Enters the quest or unlocks it if it's locked. Does nothing when quest was completed.
    /// </summary>
    public void Enter()
    {
        if (State == QuestState.Completed) return;
        else if (State == QuestState.Locked)
        {
            onQuestLocked?.Invoke(this);
            return;
        }

        onQuestStart?.Invoke(this);

        if (State == QuestState.Visited)
        {
            State = QuestState.InProgress;
            onReturn.Invoke();
        }
    }

    /// <summary>
    /// Runs logic when quest is activated after start cutscene.
    /// </summary>
    protected abstract void Activate();

    /// <summary>
    /// Runs logic when quest is stoped.
    /// </summary>
    protected abstract void Stop();

    /// <summary>
    /// Runs logic when quest is deactivated after end cutscene which starts after completion.
    /// </summary>
    protected abstract void Deactivate();

    /// <summary>
    /// Exits the quest. Does nothing when it was completed.
    /// </summary>
    public void Exit()
    {
        if(State == QuestState.Completed) return;

        onQuestStop?.Invoke(this);
        State = QuestState.Visited;
        Stop();
        onExit.Invoke();
    }

    /// <summary>
    /// Completes the quest.
    /// </summary>
    public void Complete()
    {
        onQuestEnd?.Invoke(this);
    }

    /// <summary>
    /// Skips current cutscene.
    /// </summary>
    public void SkipCutscene()
    {
        _skipCutscene = _currentCutscene != null;
        _currentCutscene?.Skip();
    }

    /// <summary>
    /// Waits for quest to start and activate after start cutscene
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitBeforeStart()
    {
        onFirstEnter.Invoke();
        yield return WaitForCutscene(_startCutscenes);
        onActivate.Invoke();
        Activate();
        if (State < QuestState.InProgress)
            State = QuestState.InProgress;
    }

    /// <summary>
    /// Waits for quest to end and deactivate after end cutscene
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitAfterComplete()
    {
        onComplete.Invoke();
        State = QuestState.Completed;
        yield return WaitForCutscene(_endCutscenes);
        Deactivate();
        onDeactivate.Invoke();
    }

    /// <summary>
    /// Waits for cutscenes to end.
    /// </summary>
    /// <param name="cutscenes">Cutscenes to run and wait for.</param>
    /// <returns></returns>
    private IEnumerator WaitForCutscene(Cutscene[] cutscenes)
    {
        foreach (var cutscene in cutscenes)
        {
            _currentCutscene = cutscene;
            if (_skipCutscene)
                cutscene.Skip();
            else 
                yield return cutscene.Play();
        }

        _currentCutscene = null;
        _skipCutscene = false;
    }

    /// <summary>
    /// Checks players action made to complete the quest.
    /// </summary>
    protected abstract void Check();

    /// <summary>
    /// Runs logic when player grabs interactable object.
    /// </summary>
    /// <param name="args"></param>
    internal virtual void OnItemGrab(SelectEnterEventArgs args) { }

    /// <summary>
    /// Runs logic when player releases interactable object.
    /// </summary>
    /// <param name="args"></param>
    internal virtual void OnItemLettingGo(SelectExitEventArgs args) { }

    /// <summary>
    /// Runs logic when player starts teleportation.
    /// </summary>
    /// <param name="provider"></param>
    internal virtual void OnTeleportStart(LocomotionProvider provider) { }

    /// <summary>
    /// Runs logic when player ends teleportaion.
    /// </summary>
    /// <param name="provider"></param>
    internal virtual void OnTeleportEnd(LocomotionProvider provider) { }
}
