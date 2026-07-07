using TMPro;
using UnityEngine;

/// <summary>
/// Represents the side of dictionary book page as an UI image with two text elements: title and description.
/// </summary>
public class DictionaryPageSide : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title, _description;

    private void Awake()
    {
        _title.text = "";
        _description.text = "";
    }

    /// <summary>
    /// Insert the word title and description in their own text fields.
    /// </summary>
    /// <param name="word"></param>
    public void SetTitle(WordData word)
    {
        _title.text = word.Title;
        _description.text = word.Description;
    }

    /// <summary>
    /// Insert the word description in its text field. Hides word title under the question mark.
    /// </summary>
    /// <param name="word">Word with description to write.</param>
    public void SetDefinition(WordData word)
    {
        _title.text = "?";
        _description.text = word.Description;
    }
}
