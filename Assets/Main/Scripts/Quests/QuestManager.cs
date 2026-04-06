using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager: MonoBehaviour
{
    [SerializeField] private QuestData _firstQuest;
    private Dictionary<QuestData, QuestState> _questStates;
    public event Action onQuestComplete;
    public event Action onQuestStart;

    public static QuestManager Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        _questStates = new Dictionary<QuestData, QuestState>();
        AddQuestToDictionary(_firstQuest);
    }

    public QuestState? GetQuestState(QuestData quest) => _questStates.ContainsKey(quest) ? _questStates[quest] : null;

    public void StartQuest(QuestData quest)
    {
        ChangeQuestState(quest, QuestState.InProgress);
        onQuestStart?.Invoke();
    }

    public void CompleteQuest(QuestData quest)
    {
        ChangeQuestState(quest, QuestState.Completed);
        onQuestComplete?.Invoke();
    }

    private void ChangeQuestState(QuestData quest, QuestState state)
    {
        if (_questStates.ContainsKey(quest))
            _questStates[quest] = state;
        else
            _questStates.Add(quest, state);
    }

    private void AddQuestToDictionary(QuestData quest, bool first = false)
    {
        _questStates.Add(quest, first ? QuestState.Available : QuestState.Locked);
        foreach(var nextQuest in quest.Next) 
            AddQuestToDictionary(nextQuest);
    }
}
