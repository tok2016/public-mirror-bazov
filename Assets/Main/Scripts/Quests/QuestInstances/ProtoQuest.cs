using TMPro;
using UnityEngine;

public abstract class ProtoQuest : Quest
{
    [SerializeField] private TextMeshProUGUI _title;

    public override void Enter()
    {
        base.Enter();
        _title.text = _data.Name;
        _title.fontStyle = FontStyles.Normal;
    }

    public override void Complete()
    {
        if (!Next)
        {
            _title.text = " вест завершен";
            _title.color = Color.green;
        }
        else
            _title.fontStyle = FontStyles.Strikethrough;

        base.Complete();
    }
}
