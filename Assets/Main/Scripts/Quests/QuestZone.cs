using UnityEngine;
using UnityEngine.Events;

public class QuestZone : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            onEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
            onExit.Invoke();
    }
}
