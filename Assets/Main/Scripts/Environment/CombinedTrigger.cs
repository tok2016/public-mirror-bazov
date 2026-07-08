using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Combines several intercepting trigger colliders in one enitre zone.
/// Ignores static objects and trigger colliders.
/// </summary>
public class CombinedTrigger : MonoBehaviour
{
    private Dictionary<Collider, int> _touchingColliders;
    public event Action<Collider> OnTriggerGroupEnter, OnTriggerGroupExit;

    /// <summary>
    /// All trigger colliders of object that were combined.
    /// </summary>
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

    /// <summary>
    /// Triggers enter event.
    /// </summary>
    /// <param name="other">Collider that entered the combined trigger.</param>
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

    /// <summary>
    /// Triggers exit event in the next frame. Handling player exit has the highest priority.
    /// If the coroutines count is higher than its max value, triggers event immediatly.
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// Waits for the next frame to exit the trigger.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private IEnumerator<int> DelayExitTrigger(Collider other)
    {
        yield return 0;
        if(_touchingColliders.ContainsKey(other))
            ExitTrigger(other);
        _coroutineCount--;
    }

    /// <summary>
    /// Handles exit event.
    /// </summary>
    /// <param name="other"></param>
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
