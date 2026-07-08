using UnityEngine;

/// <summary>
/// Triggers pad entrance and exit when player interacts with the zone.
/// </summary>
public class TeleportPadZone : MonoBehaviour
{
    [SerializeField] private TeleportPad _pad;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            _pad.EnterThePad();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _pad.ExitThePad();
    }
}
