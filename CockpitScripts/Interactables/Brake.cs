using UnityEngine;

public class Brake : MonoBehaviour
{
    [SerializeField] AirplaneController airplaneController;
    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        interactable.OnComplete += airplaneController.EngageBrakes;
    }

    private void OnDisable()
    {
        interactable.OnComplete -= airplaneController.EngageBrakes;
    }
}
