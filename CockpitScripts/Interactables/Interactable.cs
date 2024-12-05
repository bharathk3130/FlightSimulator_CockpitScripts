using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public event Action OnComplete = delegate { };
    public event Action OnEnable = delegate { };

    [SerializeField] Material highlightedMaterial;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] FlightStateManager flightStateManager;

    Material[] defaultMaterials;

    [SerializeField] bool changeOnlyFirstMat = false;

    // Used gets reset to false every time the state changes so no lever/button should be used twice in a state
    bool used;

    private void Awake()
    {
        defaultMaterials = meshRenderer.materials;
    }

    private void Start()
    {
        FlightStateManager.CurrentState.AddListener(_ => ResetButton());
    }

    [ContextMenu("Complete interaction")]
    public void ButtonPressed()
    {
        if (used) return;

        used = true;
        OnComplete(); // Calls FlightFSM.UpdateInteractable()
        ResetButtonHighlight(); // Change colour back to default as it's been pressed
    }

    [ContextMenu("Highlight Interactable")]
    void HighlightButton()
    {
        if (meshRenderer.materials.Length == 1 || changeOnlyFirstMat)
        {
            meshRenderer.material = highlightedMaterial;
        }
        else
        {
            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                materials[i] = highlightedMaterial;
            }
            meshRenderer.materials = materials;
        }
    }

    [ContextMenu("Reset Highlight")]
    void ResetButtonHighlight()
    {
        if (defaultMaterials.Length == 1 || changeOnlyFirstMat)
        {
            meshRenderer.material = defaultMaterials[0];
        }
        else
        {
            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                materials[i] = defaultMaterials[i];
            }
            meshRenderer.materials = materials;
        }
    }

    public void EnableInteractable()
    {
        OnEnable(); // Calls PushButton.EnableSelf() to let it start giving input
        HighlightButton();
    }

    void ResetButton() => used = false;
}