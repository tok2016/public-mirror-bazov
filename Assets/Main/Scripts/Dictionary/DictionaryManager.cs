using System;
using System.Collections.Generic;

/// <summary>
/// Stores dictionary words.
/// </summary>
public static class DictionaryManager
{
    public static event Action<WordData> OnWordWrite;

    /// <value>
    /// Words that are marked as written.
    /// </value>
    public static List<WordData> StoredWords { get; } = new List<WordData>();

    /// <summary>
    /// Stores the word as written and translates it to subscribed classes.
    /// </summary>
    /// <param name="wordData"></param>
    public static void WriteWord(WordData wordData)
    {
        var index = StoredWords.IndexOf(wordData);
        if(index < 0)
        {
            StoredWords.Add(wordData);
            OnWordWrite?.Invoke(wordData);
        }
    }

    /// <summary>
    /// Check if the word was stored/written.
    /// </summary>
    /// <param name="wordData">Word to check.</param>
    /// <returns>Return true if the word is stored.</returns>
    public static bool IsWordStored(WordData wordData) => StoredWords.Contains(wordData);
}
