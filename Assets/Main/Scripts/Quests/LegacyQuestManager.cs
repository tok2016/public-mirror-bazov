using System;
using System.Collections.Generic;
using UnityEngine;

public class LegacyQuestManager: MonoBehaviour
{
    [SerializeField] private LegacyQuestData _firstQuest;
    private Dictionary<LegacyQuestData, QuestState> _questStates;
    public event Action onQuestComplete;
    public event Action onQuestStart;

    public static LegacyQuestManager Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _questStates = new Dictionary<LegacyQuestData, QuestState>();
        AddQuestToDictionary(_firstQuest);
    }

    void Start()
    {

    }

    public QuestState? GetQuestState(LegacyQuestData quest) => _questStates.ContainsKey(quest) ? _questStates[quest] : null;

    public void StartQuest(LegacyQuestData quest)
    {
        ChangeQuestState(quest, QuestState.InProgress);
        onQuestStart?.Invoke();
    }

    public void CompleteQuest(LegacyQuestData quest)
    {
        ChangeQuestState(quest, QuestState.Completed);
        onQuestComplete?.Invoke();
    }

    private void ChangeQuestState(LegacyQuestData quest, QuestState state)
    {
        if (_questStates.ContainsKey(quest))
            _questStates[quest] = state;
        else
            _questStates.Add(quest, state);
    }

    private void AddQuestToDictionary(LegacyQuestData quest, bool first = false)
    {
        _questStates.Add(quest, first ? QuestState.Available : QuestState.Locked);
        foreach(var nextQuest in quest.Next) 
            AddQuestToDictionary(nextQuest);
    }
}
