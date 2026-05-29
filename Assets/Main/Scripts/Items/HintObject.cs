using UnityEngine;

public class HintObject : MonoBehaviour
{
    private MeshRenderer _renderer;

    [SerializeField] private bool _enableOutline;
    private LayerMask _hintLayer = 0;
    private LayerMask _defaultLayerMask;

    [SerializeField] private Material _hintMaterial;
    private Material _defaultMaterial;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        _defaultLayerMask = _renderer?.gameObject.layer ?? gameObject.layer;
        _defaultMaterial = _renderer?.material;
    }

    public void ToggleOutline(bool enable)
    {
        if (_enableOutline && _renderer)
            _renderer.gameObject.layer = enable ? _hintLayer : _defaultLayerMask;
    }

    public void ToggleMaterial(bool enable)
    {
        if (_hintMaterial && _defaultMaterial && _renderer)
            _renderer.material = enable ? _hintMaterial : _defaultMaterial;
    }
}
