using System;
using UnityEngine;

public enum Character
{
    Darenka = 0,
    Murenka = 1,
    Kokovanya = 2,
    Kopytce = 3,
    Bazhov = 4
}

[Serializable]
public struct DialogueLine
{
    public Character character;
    public AudioClip clip;
    [TextArea] public string text;
}
