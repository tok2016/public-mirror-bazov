using UnityEngine;

/// <summary>
/// Wraps up <c>ParticleSystem</c> to dynamicly change its properties.
/// </summary>
public class DynamicParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem.MinMaxGradient _targetColor;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void ChangeColor()
    {
        var main = _particleSystem.main;
        main.startColor = _targetColor;
    }
}
