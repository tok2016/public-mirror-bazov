using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private SceneBootstrapManager _bootstrapManager;
    [SerializeField] private int _sceneToPort;
    [SerializeField] private TeleportPad _pad;

    private void OnEnable()
    {
        _pad.onPadEnter.AddListener(TeleportToScene);
    }

    private void TeleportToScene(TeleportPad pad)
    {
        Debug.Log(pad);
        _bootstrapManager.LoadScene(_sceneToPort);
    }

    private void OnDisable()
    {
        _pad.onPadEnter.RemoveListener(TeleportToScene);
    }
}
