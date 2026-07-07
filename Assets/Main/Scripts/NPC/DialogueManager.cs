using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Distributes dialogue lines among characters. 
/// </summary>
public static class DialogueManager
{
    private static Dictionary<Character, NpcContoller> s_Characters = new Dictionary<Character, NpcContoller>();

    /// <summary>
    /// Adds given NPC controller to characters collection.
    /// </summary>
    /// <param name="character">Character to add.</param>
    /// <param name="contoller">Controller to pin to given character.</param>
    public static void AddCharacter(Character character, NpcContoller contoller)
    {
        s_Characters.Add(character, contoller);
    }

    /// <summary>
    /// Removes given character from collection.
    /// </summary>
    /// <param name="character">Character to remove.</param>
    public static void RemoveCharacter(Character character)
    {
        s_Characters.Remove(character);
    }

    /// <summary>
    /// Makes characters play given line.
    /// </summary>
    /// <param name="line">Line to play with defined character.</param>
    /// <returns>Character contoller's audio source. Return null if the character was not found.</returns>
    public static AudioSource PlayLine(DialogueLine line)
    {
        if (s_Characters.ContainsKey(line.Character))
        {
            s_Characters[line.Character].PlayLine(line);

            if (line.Word)
                DictionaryManager.WriteWord(line.Word);

            return s_Characters[line.Character].NpcAudioSource;
        }
        return null;
    }

    /// <summary>
    /// Shuts all characters. 
    /// </summary>
    public static void StopLines()
    {
        foreach (var character in s_Characters.Values)
            character.StopLine();
    }

    /// <summary>
    /// Makes defined character stop playing given line.
    /// </summary>
    /// <param name="line">Line to stop with defined character.</param>
    /// <returns>Character contoller's audio source. Return null if the character was not found.</returns>
    public static AudioSource StopLine(DialogueLine line)
    {
        if (s_Characters.ContainsKey(line.Character))
        {
            s_Characters[line.Character].StopLine(line);
            return s_Characters[line.Character].NpcAudioSource;
        }

        return null;
    }
}
