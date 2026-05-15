using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bag : MonoBehaviour
{
    private XRSocketInteractor _sokcet;

    private void Awake()
    {
        _sokcet = GetComponent<XRSocketInteractor>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
