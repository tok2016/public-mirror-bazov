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
    [SerializeField] protected ItemActiveZone _itemActiveZone;
    [SerializeField] protected PlayableDirector _startCutscene, _endCutscene;

    private bool _isCutsceneRunning = false;
    private bool _skipCutscene = false;

    public QuestState State { get; protected set; } = QuestState.Locked;

    [Header("Enter")]
    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onBeforeEnter;

    [Header("Exit")]
    public UnityEvent onExit;

    [Header("Complete")]
    public UnityEvent onComplete;
    public UnityEvent onBeforeComplete;

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
            QuestManager.EnqueueQuest(this);
            return;
        }

        _itemActiveZone.ToggleActive(true);
        QuestManager.StartQuest(this);

        if (State == QuestState.Visited)
        {
            State = QuestState.InProgress;
            onEnterRepeat.Invoke();
            Debug.Log("Return");
        }
    }

    protected abstract void Activate();
    protected abstract void Stop();
    protected abstract void Deactivate();

    public void Exit()
    {
        if(State == QuestState.Completed) return;

        QuestManager.StopQuest(this);
        State = QuestState.Visited;
        Stop();
        onExit.Invoke();
        _itemActiveZone.ToggleActive(false);
    }

    public void Complete()
    {
        QuestManager.CompleteQuest(this);
        _itemActiveZone.ToggleActive(false);
    }

    public void SkipCutscene()
    {
        _skipCutscene = _isCutsceneRunning;
    }

    public IEnumerator WaitBeforeStart()
    {
        onBeforeEnter.Invoke();
        yield return WaitForCutscene(_data.StartPhrase, _startCutscene);
        onFirstEnter.Invoke();
        Activate();
        if (State < QuestState.InProgress)
            State = QuestState.InProgress;
    }

    public IEnumerator WaitAfterComplete()
    {
        onBeforeComplete.Invoke();
        State = QuestState.Completed;
        yield return WaitForCutscene(_data.EndPhrase, _endCutscene);
        Deactivate();
        onComplete.Invoke();
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

            cutscene.Stop();
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
