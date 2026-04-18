using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ProtoQuest : Quest
{
    [SerializeField] private TextMeshProUGUI _title;

    public override void Enter()
    {
        base.Enter();
        _title.text = _data.Name;
        _title.fontStyle = FontStyles.Normal;
    }

    public override void Check(SelectEnterEventArgs args) { }

    public override void Complete()
    {
        if (_data.Next.Length == 0)
        {
            _title.text = " вест завершен";
            _title.color = Color.green;
        }
        else
            _title.fontStyle = FontStyles.Strikethrough;

        base.Complete();
    }
}
