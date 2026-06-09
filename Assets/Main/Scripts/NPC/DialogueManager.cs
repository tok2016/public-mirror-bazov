using System;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueManager
{
    private static Dictionary<Character, NpcContoller> _characters = new Dictionary<Character, NpcContoller>();

    public static void AddCharacter(Character character, NpcContoller contoller)
    {
        _characters.Add(character, contoller);
    }

    public static void RemoveCharacter(Character character)
    {
        _characters.Remove(character);
    }

    public static void PlayLine(DialogueLine line)
    {   
        if (_characters.ContainsKey(line.character))
            _characters[line.character].Pronounce(line.clip);
    }
}
