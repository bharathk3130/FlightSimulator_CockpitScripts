using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class EngineButton : MonoBehaviour
{
    [SerializeField] FlightStateManager flightStateManager;

    Interactable interactable;

    bool isEnabled;

    void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnEnable += EnableSelf;
    }

    [ContextMenu("Enable button")]
    void EnableSelf() => isEnabled = true;

    public void ButtonPressed()
    {
        if (!isEnabled) return;

        interactable.ButtonPressed();
    }
}
