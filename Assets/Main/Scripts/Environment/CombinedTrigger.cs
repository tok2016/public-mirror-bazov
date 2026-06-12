using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinedTrigger : MonoBehaviour
{
    private Dictionary<Collider, int> _touchingColliders;
    public event Action<Collider> OnTriggerGroupEnter, OnTriggerGroupExit;
    public Collider[] Triggers { get; private set; }
    private int _maxCoroutinesCount = 3;
    private int _coroutineCount = 0;

    private void Awake()
    {
        _touchingColliders = new Dictionary<Collider, int>();
        Triggers = GetComponentsInChildren<Collider>()
            .Where(trigger => trigger.isTrigger)
            .ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.isStatic || other.isTrigger) return;

        if (Triggers.Length < 2)
        {
            OnTriggerGroupEnter?.Invoke(other);
            return;
        }

        if (_touchingColliders.ContainsKey(other))
            _touchingColliders[other]++;
        else
        {
            _touchingColliders[other] = 1;
            OnTriggerGroupEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.isStatic || other.isTrigger) return;

        if (Triggers.Length < 2)
        {
            OnTriggerGroupExit?.Invoke(other);
            return;
        }

        if (_touchingColliders.ContainsKey(other))
        {
            if (_coroutineCount < _maxCoroutinesCount || other.tag == "Player")
            {
                _coroutineCount++;
                StartCoroutine(DelayExitTrigger(other));
            }
            else
                ExitTrigger(other);
        }
    }

    private IEnumerator<int> DelayExitTrigger(Collider other)
    {
        yield return 0;
        if(_touchingColliders.ContainsKey(other))
            ExitTrigger(other);
        _coroutineCount--;
    }

    private void ExitTrigger(Collider other)
    {
        _touchingColliders[other]--;
        if (_touchingColliders[other] == 0)
        {
            _touchingColliders.Remove(other);
            OnTriggerGroupExit?.Invoke(other);

            if(other.tag == "Player")
                _touchingColliders.Clear();
        }
    }

    private void OnDisable()
    {
        _touchingColliders.Clear();
    }
}
