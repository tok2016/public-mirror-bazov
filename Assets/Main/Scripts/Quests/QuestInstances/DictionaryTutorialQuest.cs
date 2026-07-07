using UnityEngine;

/// <summary>
/// Checks for dictionary usage quest completion.
/// </summary>
public class DictionaryTutorialQuest : TutorialQuest
{
    [SerializeField] private DictionaryBook _dictionary;

    private void OnEnable()
    {
        _dictionary.onPageTurn += OnDictionaryPageTurn;
    }

    /// <summary>
    /// Checks if player has turned the page.
    /// </summary>
    /// <param name="page">Page player has turned</param>
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
