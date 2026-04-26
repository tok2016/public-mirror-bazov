using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class NpcGrabbable : MonoBehaviour
{
    public UnityEvent onGrab;
    public UnityEvent onLettingGo;
    public UnityEvent onAttach;

    public bool IsGrabbed {  get; private set; }

    [field: SerializeField] public bool IsForPlayer {  get; private set; }

    public void Grab()
    {
        onGrab.Invoke();
        IsGrabbed = true;
    }

    public void LetGo()
    {
        onLettingGo.Invoke();
        IsGrabbed = false;
    }

    public void Attach()
    {
        onAttach.Invoke();
        Destroy(gameObject);
    }
}
