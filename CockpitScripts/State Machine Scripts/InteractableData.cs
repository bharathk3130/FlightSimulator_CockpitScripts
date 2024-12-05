using System;
using UnityEngine;

[Serializable]
public struct Interactableitem
{
    public Interactable interactable;
    public string text;
}

public class InteractableData : MonoBehaviour
{

    public static InteractableData Instance { get; private set; }


    [SerializeField] Interactableitem[] startStateItems;
    [SerializeField] Interactableitem[] takeOffStateItems;
    [SerializeField] Interactableitem[] climbStateItems;
    [SerializeField] Interactableitem[] cruiseStateItems;
    [SerializeField] Interactableitem[] descentStateItems;
    [SerializeField] Interactableitem[] approachStateItems;
    [SerializeField] Interactableitem[] landingStateItems;
    [SerializeField] Interactableitem[] switchoffStateItems;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void InitializeInteractables(Interactableitem[] interactableItems, Action OnComplete)
    {
        if (interactableItems == null) return;
        
        foreach (var item in interactableItems)
        {
            if (item.interactable != null)
                item.interactable.OnComplete += OnComplete;
        }
    }

    public Interactableitem[] GetInteractableItemArray(State state)
    {
        return state switch
        {
            State.Start => startStateItems,
            State.TakeOff => takeOffStateItems,
            State.Climb => climbStateItems,
            State.Cruise => cruiseStateItems,
            State.Descent=> descentStateItems,
            State.Approach => approachStateItems,
            State.Landing => landingStateItems,
            State.SwitchOff => switchoffStateItems,
            // Other states
            _ => HandleError(state)
        };
    }

    Interactableitem[] HandleError(State state)
    {
        Debug.LogError($"{state} not implemented in InteractableData");
        return null;
    }

}
