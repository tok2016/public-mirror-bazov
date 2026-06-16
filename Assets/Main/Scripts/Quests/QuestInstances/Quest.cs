using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public abstract class Quest : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] protected QuestData _data;
    [field: SerializeField] public Quest Next {  get; private set; }
    [SerializeField] protected PlayableDirector _startCutscene, _endCutscene;

    private bool _isCutsceneRunning = false;
    private bool _skipCutscene = false;

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
            //QuestManager.EnqueueQuest(this);
            onQuestLocked?.Invoke(this);
            return;
        }

        //QuestManager.StartQuest(this);
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

        //QuestManager.StopQuest(this);
        onQuestStop?.Invoke(this);
        State = QuestState.Visited;
        Stop();
        onExit.Invoke();
    }

    public void Complete()
    {
        //QuestManager.CompleteQuest(this);
        onQuestEnd?.Invoke(this);
    }

    public void SkipCutscene()
    {
        _skipCutscene = _isCutsceneRunning;
    }

    public IEnumerator WaitBeforeStart()
    {
        onFirstEnter.Invoke();
        yield return WaitForCutscene(_data.StartPhrase, _startCutscene);
        onActivate.Invoke();
        Activate();
        if (State < QuestState.InProgress)
            State = QuestState.InProgress;
    }

    public IEnumerator WaitAfterComplete()
    {
        onComplete.Invoke();
        State = QuestState.Completed;
        yield return WaitForCutscene(_data.EndPhrase, _endCutscene);
        Deactivate();
        onDeactivate.Invoke();
    }

    private IEnumerator WaitForCutscene(DialogueLine phrase, PlayableDirector cutscene)
    {
        _isCutsceneRunning = true;
        if (phrase)
        {
            var timer = phrase.Clip.length;
            DialogueManager.PlayLine(phrase);
            while (timer > 0 && !_skipCutscene)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            DialogueManager.StopLines();
        }

        if (cutscene)
        {
            cutscene.Play();
            var cutsceneDuration = Mathf.Ceil((float)cutscene.duration);
            while (cutsceneDuration > 0 && !_skipCutscene)
            { 
                cutsceneDuration -= Time.deltaTime;
                yield return null;
            }

            if (_skipCutscene)
            {
                var diff = cutscene.duration - cutscene.time;
                cutscene.time += diff;
                cutscene.Evaluate();
            }
        }

        _isCutsceneRunning = false;
        _skipCutscene = false;
    }

    protected abstract void Check();

    internal virtual void OnItemGrab(SelectEnterEventArgs args) { }

    internal virtual void OnItemLettingGo(SelectExitEventArgs args) { }

    internal virtual void OnTeleportStart(LocomotionProvider provider) { }

    internal virtual void OnTeleportEnd(LocomotionProvider provider) { }
}
