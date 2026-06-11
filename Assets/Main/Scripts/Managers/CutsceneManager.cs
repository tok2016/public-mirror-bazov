using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private Queue<Quest> _questsQueue;
    private Coroutine coroutine;

    private void Awake()
    {
        _questsQueue = new Queue<Quest>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private IEnumerator<Coroutine> Rh()
    {
        yield return coroutine;
    }
}
