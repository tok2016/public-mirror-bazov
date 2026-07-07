using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// Manages the queue of quests player has entered.
/// </summary>
public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest[] _quests;

    [Header("Input")]
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private XRBaseInteractor _leftGrabInteractor, _rightGrabInteractor;
    [SerializeField] private XRBaseInteractor[] _otherGrabInteractors;
    [SerializeField] private InputActionReference _skipAction;

    [Header("Quest UI")]
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questDescription;

    private Coroutine _delayRoutine;

    private Queue<Quest> _enteredQuests = new Queue<Quest>();
    private Quest _current;
    private bool _IsOccupied = false;

    /// <value>
    /// Maps QuestState value to it's text version
    /// </value>
    public readonly static Dictionary<QuestState, string> QuestStateNames = new Dictionary<QuestState, string>() 
    { 
        { QuestState.Locked, "Недоступен"},
        { QuestState.Available, "В процессе" },
        { QuestState.Visited, "В процессе" },
        { QuestState.InProgress, "В процессе" },
        { QuestState.Completed, "Завершён" }
    };

    private void Start()
    {
        UpdateQuestInfo(null);
    }

    private void OnEnable()
    {
        foreach (var quest in _quests)
        {
            quest.onQuestLocked += EnqueueLockedQuest;
            quest.onQuestStart += StartQuest;
            quest.onQuestStop += StopQuest;
            quest.onQuestEnd += EndQuest;
        }

        _teleportationProvider.locomotionStarted += OnQuestTeleportationStart;
        _teleportationProvider.locomotionEnded += OnQuestTeleportationEnd;

        _leftGrabInteractor.selectEntered.AddListener(OnQuestItemGrab);
        _leftGrabInteractor.selectExited.AddListener(OnQuestItemLetGo);

        _rightGrabInteractor.selectEntered.AddListener(OnQuestItemGrab);
        _rightGrabInteractor.selectExited.AddListener(OnQuestItemLetGo);

        _skipAction.action.performed += SkipCutscene;

        foreach(var interactor  in _otherGrabInteractors)
        {
            interactor.selectEntered.AddListener(OnQuestItemGrab);
            interactor.selectExited.AddListener(OnQuestItemLetGo);
        }
    }

    /// <summary>
    /// Displays title and description of quest in pause menu. If given quest is completed, it displays a message about it's completion.
    /// </summary>
    /// <param name="quest">Quest with data to display. If it's null, method displays the default message.</param>
    private void UpdateQuestInfo(Quest quest)
    {
        if(quest == null || quest.Data == null)
        {
            _questTitle.text = "Нет доступных заданий";
            _questDescription.text = "Проходите дальше по квесту, чтобы получить больше заданий";
        } 
        else
        {
            _questTitle.text = $"{QuestStateNames[quest.State]}: {quest.Data.Name}";
            _questDescription.text = quest.State == QuestState.Completed ? quest.Data.CompleteMessage : quest.Data.Description;
        }
    }

    /// <summary>
    /// Pushes quest to queue. If no quest or cutscene is currently active, it unlocks and enters given quest.
    /// </summary>
    /// <param name="quest">Quest to enqueue</param>
    private void EnqueueLockedQuest(Quest quest)
    {
        if (_current == null && _delayRoutine == null)
        {
            quest.Unlock();
            quest.Enter();
        }
        else
            _enteredQuests.Enqueue(quest);
    }

    /// <summary>
    /// Activates given quests and waits for its start. If one of the quests is still active, it enqueues given one.
    /// </summary>
    /// <param name="quest">Quest to activate and start</param>
    private void StartQuest(Quest quest)
    {
        if(_current != null)
        {
            EnqueueLockedQuest(quest);
            return;
        }

        quest.gameObject.SetActive(true);
        _current = quest;
        UpdateQuestInfo(_current);
        StartCoroutine(WaitForQuestStart(quest));
    }

    /// <summary>
    /// Stops given quest and dequeues the next one.
    /// </summary>
    /// <param name="quest">Quest to stop</param>
    private void StopQuest(Quest quest)
    {
        quest.gameObject.SetActive(false);
        UpdateQuestInfo(quest);

        if (quest == _current)
            _current = null;

        if (_enteredQuests.Count == 0) return;
        else if (_enteredQuests.Count > 1)
        {
            var tempQueue = new Queue<Quest>();
            while (_enteredQuests.Count > 0)
            {
                var currentQuest = _enteredQuests.Dequeue();
                if (currentQuest != quest)
                    tempQueue.Enqueue(currentQuest);
            }

            _enteredQuests = tempQueue;
        }

        _enteredQuests.Dequeue().Enter();
    }

    /// <summary>
    /// Waits for given quest to end
    /// </summary>
    /// <param name="quest">Quest to end</param>
    private void EndQuest(Quest quest)
    {
        StartCoroutine(WaitForQuestComplete(quest));
    }

    /// <summary>
    /// Skips cutscene of currently active quest
    /// </summary>
    /// <param name="context">Input action context</param>
    private void SkipCutscene(InputAction.CallbackContext context)
    {
        _current?.SkipCutscene();
    }

    /// <summary>
    /// Waits for given quest to start if it's available
    /// </summary>
    /// <param name="quest">Quest waiting for the start</param>
    /// <returns></returns>
    private System.Collections.IEnumerator WaitForQuestStart(Quest quest)
    {
        _IsOccupied = true;
        if (quest.State == QuestState.Available)
            yield return quest.WaitBeforeStart();
        _IsOccupied = false;
    }

    /// <summary>
    /// Waits for given quest to end after completion
    /// </summary>
    /// <param name="quest">Quest waiting for the end</param>
    /// <returns></returns>
    private System.Collections.IEnumerator WaitForQuestComplete(Quest quest)
    {
        while (_IsOccupied)
            yield return null;

        yield return quest.WaitAfterComplete();
        quest.Next?.Unlock();
        StopQuest(quest);
    }

    /// <summary>
    /// Delays the start of quests player has entered
    /// </summary>
    /// <param name="delay">Delay time in seconds</param>
    public void Delay(float delay)
    {
        if (_delayRoutine != null)
        {
            StopCoroutine(_delayRoutine);
            _delayRoutine = null;
        }
        _delayRoutine = StartCoroutine(DelayRoutine(delay));
    }

    /// <summary>
    /// Waits for delay to be completed
    /// </summary>
    /// <param name="delay">Delay time in seconds</param>
    /// <returns></returns>
    private System.Collections.IEnumerator DelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _delayRoutine = null;

        if (_enteredQuests.Count > 0)
            _enteredQuests.Dequeue().Enter();
    }

    /// <summary>
    /// Runs logic specific to current quest when player grabs interactable object
    /// </summary>
    /// <param name="args"></param>
    public void OnQuestItemGrab(SelectEnterEventArgs args)
    {
        _current?.OnItemGrab(args);
    }

    /// <summary>
    /// Runs logic specific to current quest when player releases interactable object
    /// </summary>
    /// <param name="args"></param>
    public void OnQuestItemLetGo(SelectExitEventArgs args)
    {
        _current?.OnItemLettingGo(args);
    }

    /// <summary>
    /// Runs logic specific to current quest when player starts teleportation
    /// </summary>
    /// <param name="provider"></param>
    public void OnQuestTeleportationStart(LocomotionProvider provider)
    {
        _current?.OnTeleportStart(provider);
    }

    /// <summary>
    /// Runs logic specific to current quest when player ends teleportation
    /// </summary>
    /// <param name="provider"></param>
    public void OnQuestTeleportationEnd(LocomotionProvider provider)
    {
        _current?.OnTeleportEnd(provider);
    }

    private void OnDisable()
    {
        foreach (var quest in _quests)
        {
            quest.onQuestLocked -= EnqueueLockedQuest;
            quest.onQuestStart -= StartQuest;
            quest.onQuestStop -= StopQuest;
            quest.onQuestEnd -= EndQuest;
        }

        _teleportationProvider.locomotionStarted -= OnQuestTeleportationStart;
        _teleportationProvider.locomotionEnded -= OnQuestTeleportationEnd;

        _leftGrabInteractor.selectEntered.RemoveListener(OnQuestItemGrab);
        _leftGrabInteractor.selectExited.RemoveListener(OnQuestItemLetGo);

        _rightGrabInteractor.selectEntered.RemoveListener(OnQuestItemGrab);
        _rightGrabInteractor.selectExited.RemoveListener(OnQuestItemLetGo);

        _skipAction.action.performed -= SkipCutscene;

        foreach (var interactor in _otherGrabInteractors)
        {
            interactor.selectEntered.RemoveListener(OnQuestItemGrab);
            interactor.selectExited.RemoveListener(OnQuestItemLetGo);
        }
    }
}
