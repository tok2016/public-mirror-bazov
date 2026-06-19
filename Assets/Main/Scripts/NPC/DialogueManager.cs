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

    public static AudioSource PlayLine(DialogueLine line)
    {
        if (_characters.ContainsKey(line.Character))
        {
            _characters[line.Character].PlayLine(line);

            //if (line.Word)
            //    DictionaryManager.WriteWord(line.Word);

            return _characters[line.Character].AudioSource;
        }
        return null;
    }

    public static void StopLines()
    {
        foreach (var character in _characters.Values)
            character.ShutUp();
    }
}
