using UnityEngine;

public class FlightFSM 
{

    protected FlightStateManager stateManager;
    protected InteractableData interactableData;

    protected Interactableitem[] interactableItems;
    protected int currentIndex = -1;

    protected State state;

    protected FlightValuesSO flightValues;

    public FlightFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues)
    {
        this.flightValues = flightValues;
        this.stateManager = stateManager;
        //    this.state = state;
        //    interactableData = InteractableData.Instance;

        //    interactableItems = interactableData.GetInteractableItemArray(state);
        //    interactableData.InitializeInteractables(interactableItems, UpdateInteractable);
    }

    public void EnteredNewState(FlightStateManager stateManager, State state)
    {
        //this.stateManager = stateManager;
        this.state = state;
        interactableData = InteractableData.Instance;

        interactableItems = interactableData.GetInteractableItemArray(state);
        interactableData.InitializeInteractables(interactableItems, UpdateInteractable);
    }

    public virtual void InitState()
    {
    }

    public virtual void UpdateState() { }

    public virtual void Update(float dt) { }

    protected void UpdateInteractable()
    {
        currentIndex++;
        if (currentIndex < interactableItems.Length)
        {
            GetCurrentInteractable()?.EnableInteractable();
            UpdatePromptUI(GetCurrentText());
        }
        else
        {
            FinishedStateInteractions();
        }
    }

    protected virtual void FinishedStateInteractions() { }

    Interactable GetCurrentInteractable() => interactableItems[currentIndex].interactable;
    string GetCurrentText()
    {
        return interactableItems[currentIndex].text;
    }

    protected void UpdatePromptUI(string text) => stateManager.prompts.text = text;
}