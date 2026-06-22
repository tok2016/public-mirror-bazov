using TMPro;
using UnityEngine;

public abstract class ProtoQuest : Quest
{
    [SerializeField] private TextMeshProUGUI _title;

    protected override void Activate()
    {
        _title.text = Data.Name;
        _title.fontStyle = FontStyles.Normal;
    }

    protected override void Stop()
    {
        
    }

    protected override void Deactivate()
    {
        if (!Next)
        {
            _title.text = " вест завершен";
            _title.color = Color.green;
        }
        else
            _title.fontStyle = FontStyles.Strikethrough;
    }
}
