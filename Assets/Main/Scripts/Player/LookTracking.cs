using UnityEngine;

/// <summary>
/// Rotates object towards the player.
/// </summary>
public class LookTracking : MonoBehaviour
{
    [SerializeField] private Transform _player;

    void Update()
    {
        transform.LookAt(_player);
    }
}
