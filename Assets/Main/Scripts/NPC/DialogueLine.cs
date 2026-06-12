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

public enum AudioPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

[CreateAssetMenu(fileName = "DialogueLine", menuName = "Scriptable Objects/DialogueLine")]
public class DialogueLine : ScriptableObject
{
    [field: SerializeField] public Character Character { get; private set; }
    [field: SerializeField] public AudioClip Clip { get; private set; }
    [field: SerializeField] public AudioPriority Priority { get; private set; }
    [field: SerializeField, TextArea] public string Text {  get; private set; }
    [field: SerializeField] public WordData Word { get; private set; }
}
