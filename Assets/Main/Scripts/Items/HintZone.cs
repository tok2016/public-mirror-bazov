using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HintZone : MonoBehaviour
{
    [SerializeField] private DialogueLine _hint;
    public Dictionary<Collider, IGrabbable> Items {  get; private set; }

    void Start()
    {
        Items = new Dictionary<Collider, IGrabbable>();
    }

    public void CommentHint()
    {
        if (_hint != null)
            DialogueManager.PlayLine(_hint);
    }

    private void OnDrawGizmos()
    {
        var collider = GetComponent<BoxCollider>();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, collider.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<IGrabbable>() ?? other.transform.parent?.GetComponent<IGrabbable>();
        if(item != null && item.Interactable.isSelected)
            Items.Add(other, item);
            
        if (other.tag == "Player" && Items.Count > 0)
            CommentHint();
    }

    private void OnTriggerExit(Collider other)
    {
        if(Items.ContainsKey(other))
            Items.Remove(other);
    }
}
