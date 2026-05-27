using UnityEngine;

public class HintObject : MonoBehaviour
{
    private MeshRenderer _renderer;
    private LayerMask _hintLayer = 6;
    private LayerMask _defaultLayerMask;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        _defaultLayerMask = _renderer == null ? gameObject.layer : _renderer.gameObject.layer;
    }

    public void ToggleHint(bool enable)
    {
        var layer = enable ? _hintLayer : _defaultLayerMask;
        if (_renderer != null)
            _renderer.gameObject.layer = layer;
        else
            gameObject.layer = layer;
    }
}
