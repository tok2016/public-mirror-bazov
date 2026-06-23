using UnityEngine;

public class DictionaryTutorialQuest : TutorialQuest
{
    [SerializeField] private DictionaryBook _dictionary;

    private void OnEnable()
    {
        _dictionary.onPageTurn += OnDictionaryPageTurn;
    }

    private void OnDictionaryPageTurn(DictionaryPage page)
    {
        if(State < QuestState.Completed)
            Check();
    }

    private void OnDisable()
    {
        _dictionary.onPageTurn -= OnDictionaryPageTurn;
    }

    protected override void ToggleControllersHint(bool enable)
    {
        
    }
}
