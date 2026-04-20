using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[System.Serializable]
public struct ProtoGemProps
{
    public TextMeshProUGUI title;
    public XRGrabInteractable correctGem;
}

[CreateAssetMenu(fileName = "ProtoGemQuest", menuName = "Scriptable Objects/Quests/ProtoGemQuest")]
public class ProtoGemQuest : Quest<ProtoGemProps>
{
    private TextMeshProUGUI _title;
    private XRGrabInteractable _correctGem;

    public override void Enter(ProtoGemProps props)
    {
        base.Enter(props);
        _correctGem = props.correctGem;
        _title = props.title;
        _title.text = Name;
        _title.fontStyle = FontStyles.Normal;
    }

    public override void Check(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == _correctGem.gameObject)
        {
            Complete();
        }
    }

    public override void Complete()
    {
        base.Complete();
        _title.text = " вест завершен";
        _title.color = Color.green;
    }
}
