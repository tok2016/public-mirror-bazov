using UnityEngine;

/// <summary>
/// Activates scene loading when player steps on it.
/// </summary>
public class Portal : MonoBehaviour
{
    [SerializeField] private SceneBootstrapManager _bootstrapManager;
    [SerializeField] private int _sceneToPort;
    [SerializeField] private TeleportPad _pad;

    private void OnEnable()
    {
        _pad.onPadEnter.AddListener(TeleportToScene);
    }

    /// <summary>
    /// Calls scene loading when player steps on pad.
    /// </summary>
    /// <param name="pad">Pad where player teleported at.</param>
    private void TeleportToScene(TeleportPad pad)
    {
        _bootstrapManager.LoadScene(_sceneToPort);
    }

    private void OnDisable()
    {
        _pad.onPadEnter.RemoveListener(TeleportToScene);
    }
}
