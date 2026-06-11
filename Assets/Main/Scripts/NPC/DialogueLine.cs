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

[CreateAssetMenu(fileName = "DialogueLine", menuName = "Scriptable Objects/DialogueLine")]
public class DialogueLine : ScriptableObject
{
    [field: SerializeField] public Character Character { get; private set; }
    [field: SerializeField] public AudioClip Clip { get; private set; }
    [field: SerializeField, TextArea] public string Text {  get; private set; }
    [field: SerializeField] public WordData Word { get; private set; }
}
