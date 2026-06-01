using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HintZone : MonoBehaviour
{
    [SerializeField, TextArea] private string _hint;
    public Dictionary<Collider, CollectableItem> Items {  get; private set; }

    void Start()
    {
        Items = new Dictionary<Collider, CollectableItem>();
    }

    public void CommentHint()
    {
        Debug.Log(_hint);
    }

    private void OnDrawGizmos()
    {
        var collider = GetComponent<BoxCollider>();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, collider.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectableItem>() ?? other.transform.parent?.GetComponent<CollectableItem>();
        if(item && !item.IsCollected && item.Interactable.interactorsSelecting.Count == 0)
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
