using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest[] _quests;
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private XRBaseInteractor _leftGrabInteractor, _rightGrabInteractor;
    [SerializeField] private XRBaseInteractor[] _otherGrabInteractors;
    [SerializeField] private InputActionReference _skipAction;

    private Queue<Quest> _enteredQuests = new Queue<Quest>();
    private Quest _current;
    private bool _IsOccupied = false;

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

    private void EnqueueLockedQuest(Quest quest)
    {
        if (_current == null)
        {
            quest.Unlock();
            quest.Enter();
        }
        else
            _enteredQuests.Enqueue(quest);
    }

    private void StartQuest(Quest quest)
    {
        quest.gameObject.SetActive(true);
        _current = quest;
        StartCoroutine(WaitForQuestStart(quest));
    }

    private void StopQuest(Quest quest)
    {
        quest.gameObject.SetActive(false);

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

        _current = _enteredQuests.Dequeue();
        _current.Enter();
    }

    private void EndQuest(Quest quest)
    {
        StartCoroutine(WaitForQuestComplete(quest));
    }

    private void SkipCutscene(InputAction.CallbackContext context)
    {
        _current?.SkipCutscene();
    }

    private System.Collections.IEnumerator WaitForQuestStart(Quest quest)
    {
        _IsOccupied = true;
        if (quest.State == QuestState.Available)
            yield return quest.WaitBeforeStart();
        _IsOccupied = false;
    }

    private System.Collections.IEnumerator WaitForQuestComplete(Quest quest)
    {
        while (_IsOccupied)
            yield return null;

        yield return quest.WaitAfterComplete();
        quest.Next?.Unlock();
        StopQuest(quest);
    }

    public void OnQuestItemGrab(SelectEnterEventArgs args)
    {
        _current?.OnItemGrab(args);
    }

    public void OnQuestItemLetGo(SelectExitEventArgs args)
    {
        _current?.OnItemLettingGo(args);
    }

    public void OnQuestTeleportationStart(LocomotionProvider provider)
    {
        _current?.OnTeleportStart(provider);
    }

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
