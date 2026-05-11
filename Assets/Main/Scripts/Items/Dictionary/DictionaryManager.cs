using System;
using System.Collections.Generic;

public static class DictionaryManager
{
    public static event Action<WordData, int> OnWordWrite;
    public static List<WordData> StoredWords { get; } = new List<WordData>();

    public static void WriteWord(WordData wordData)
    {
        var index = StoredWords.IndexOf(wordData);
        if(index < 0)
        {
            StoredWords.Add(wordData);
            OnWordWrite?.Invoke(wordData, StoredWords.Count);
        }
    }

    public static bool IsWordStored(WordData wordData) => StoredWords.Contains(wordData);
}
