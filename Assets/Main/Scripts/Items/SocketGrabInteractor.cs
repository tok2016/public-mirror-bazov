using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Interacts with grabbles and can be controlled by player. 
/// </summary>
[RequireComponent(typeof(XRSocketInteractor))]
public class SocketGrabInteractor : MonoBehaviour
{
    [SerializeField] private float _restoreTime = 1f;
    private XRSocketInteractor _socket;

    private void Awake()
    {
        _socket = GetComponent<XRSocketInteractor>();
    }

    /// <summary>
    /// Disables socket when releasing an item.
    /// </summary>
    /// <param name="args"></param>
    public void ReleaseItem(BaseInteractionEventArgs args)
    {
        if ((args.interactableObject.interactionLayers.value & _socket.interactionLayers.value) != _socket.interactionLayers.value) 
            return;

        _socket.socketActive = false;
        if (_restoreTime > 0)
            StartCoroutine(RestoreAfterTime());
    }

    /// <summary>
    /// Enable socket.
    /// </summary>
    public void RestoreSocket()
    {
        _socket.socketActive = true;
    }

    /// <summary>
    /// Delays socket restore.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreAfterTime()
    {
        yield return new WaitForSeconds(_restoreTime);
        RestoreSocket();
    }
}
