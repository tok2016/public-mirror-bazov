using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketGrabInteractor : MonoBehaviour
{
    [SerializeField] private float _restoreTime = 1f;
    private XRSocketInteractor _socket;

    private void Awake()
    {
        _socket = GetComponent<XRSocketInteractor>();
    }

    public void ReleaseItem(BaseInteractionEventArgs args)
    {
        if ((args.interactableObject.interactionLayers.value & _socket.interactionLayers.value) != _socket.interactionLayers.value) 
            return;

        _socket.socketActive = false;
        if (_restoreTime > 0)
            StartCoroutine(RestoreAfterTime());
    }

    public void RestoreSocket()
    {
        _socket.socketActive = true;
    }

    private IEnumerator RestoreAfterTime()
    {
        yield return new WaitForSeconds(_restoreTime);
        RestoreSocket();
    }
}
