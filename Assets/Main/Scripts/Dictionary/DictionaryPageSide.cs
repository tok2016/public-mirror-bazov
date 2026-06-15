using TMPro;
using UnityEngine;

public class DictionaryPageSide : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title, _description;

    private void Awake()
    {
        _title.text = "";
        _description.text = "";
    }

    public void SetTitle(WordData word)
    {
        _title.text = word.Title;
        _description.text = word.Description;
    }

    public void SetDefinition(WordData word)
    {
        _title.text = "?";
        _description.text = word.Description;
    }
}
