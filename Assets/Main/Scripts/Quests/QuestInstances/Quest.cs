using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public abstract class Quest : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] protected QuestData _data;
    [field: SerializeField] public Quest Next {  get; private set; }
    [SerializeField] protected Cutscene[] _startCutscenes, _endCutscenes;

    private bool _skipCutscene = false;
    private Cutscene _currentCutscene;

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

    public void Unlock()
    {
        State = QuestState.Available;
        gameObject.SetActive(true);
    }

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

    protected abstract void Activate();
    protected abstract void Stop();
    protected abstract void Deactivate();

    public void Exit()
    {
        if(State == QuestState.Completed) return;

        onQuestStop?.Invoke(this);
        State = QuestState.Visited;
        Stop();
        onExit.Invoke();
    }

    public void Complete()
    {
        onQuestEnd?.Invoke(this);
    }

    public void SkipCutscene()
    {
        _skipCutscene = _currentCutscene != null;
        _currentCutscene?.Skip();
    }

    public IEnumerator WaitBeforeStart()
    {
        onFirstEnter.Invoke();
        yield return WaitForCutscene(_startCutscenes);
        onActivate.Invoke();
        Activate();
        if (State < QuestState.InProgress)
            State = QuestState.InProgress;
    }

    public IEnumerator WaitAfterComplete()
    {
        onComplete.Invoke();
        State = QuestState.Completed;
        yield return WaitForCutscene(_endCutscenes);
        Deactivate();
        onDeactivate.Invoke();
    }

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

    protected abstract void Check();

    internal virtual void OnItemGrab(SelectEnterEventArgs args) { }

    internal virtual void OnItemLettingGo(SelectExitEventArgs args) { }

    internal virtual void OnTeleportStart(LocomotionProvider provider) { }

    internal virtual void OnTeleportEnd(LocomotionProvider provider) { }
}
