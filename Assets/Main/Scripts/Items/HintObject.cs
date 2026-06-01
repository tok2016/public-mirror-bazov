using UnityEngine;

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
    }

    protected virtual void Start()
    {
        _defaultLayerMask = _renderer?.gameObject.layer ?? gameObject.layer;
        _defaultMaterial = _renderer?.material;
    }

    public void ToggleOutline(bool enable)
    {
        if (_enableOutline && _renderer)
            _renderer.gameObject.layer = enable ? _hintLayer : _defaultLayerMask;
    }

    public virtual void ToggleMaterial(bool enable)
    {
        if (_hintMaterial && _defaultMaterial && _renderer)
            _renderer.material = enable ? _hintMaterial : _defaultMaterial;
    }
}
