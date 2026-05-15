using UnityEngine;

public class LookTracking : MonoBehaviour
{
    [SerializeField] private Transform _player;

    void Update()
    {
        transform.LookAt(_player);
    }
}
