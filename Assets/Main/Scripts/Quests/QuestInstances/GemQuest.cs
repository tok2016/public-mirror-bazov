using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Checks for gem searching quest to be completed.
/// </summary>
public class GemQuest : Quest
{
    [Header("Check")]
    [SerializeField] private Gem _correctGem;
    [SerializeField] private XRSocketInteractor _correctItemSocket;
    [SerializeField] private GameObject _successEffect;

    private void OnEnable()
    {
        _correctItemSocket.selectEntered.AddListener(CheckGem);
    }

    /// <summary>
    /// Compares socketed item with the correct gem.
    /// </summary>
    /// <param name="args"></param>
    private void CheckGem(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetInstanceID() == _correctGem.transform.GetInstanceID())
            Check();
    }

    protected override void Check()
    {
        _correctGem.gameObject.SetActive(false);
        _successEffect.SetActive(true);
        Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        if (State != QuestState.InProgress) return;

        var gem = args.interactableObject.transform.GetComponent<Gem>();
        if (gem)
            gem.CommentGrab();
    }

    private void OnDisable()
    {
        _correctItemSocket.selectEntered.RemoveListener(CheckGem);
    }

    protected override void Activate()
    {
        _correctItemSocket.gameObject.SetActive(true);
    }

    protected override void Stop()
    {
        
    }

    protected override void Deactivate()
    {
        
    }
}
