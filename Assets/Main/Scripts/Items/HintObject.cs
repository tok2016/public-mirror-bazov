using UnityEngine;

/// <summary>
/// Highlights <c>MonoBehavior</c> object.
/// </summary>
public class HintObject : MonoBehaviour
{
    protected MeshRenderer _renderer;

    [SerializeField] protected bool _enableOutline;
    protected LayerMask _hintLayer = 6;
    protected LayerMask _defaultLayerMask;

    [SerializeField] protected Material _hintMaterial;
    protected Material _defaultMaterial;

    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _defaultLayerMask = _renderer?.gameObject.layer ?? gameObject.layer;
        _defaultMaterial = _renderer?.material;
    }

    /// <summary>
    /// Displays or hides outline. If outline toggle is forbidden, the method does nothing. 
    /// </summary>
    /// <param name="enable">Whether to display or hide outline.</param>
    public void ToggleOutline(bool enable)
    {
        if (_enableOutline && _renderer)
            _renderer.gameObject.layer = enable ? _hintLayer : _defaultLayerMask;
    }

    /// <summary>
    /// Changes object material to highlighted one, if it was set, or resets it.
    /// </summary>
    /// <param name="enable">Whether to change material or reset it.</param>
    public virtual void ToggleMaterial(bool enable)
    {
        if (_hintMaterial && _defaultMaterial && _renderer)
            _renderer.material = enable ? _hintMaterial : _defaultMaterial;
    }
}
