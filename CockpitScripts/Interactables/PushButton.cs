using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Interactable))]
public class PushButton : MonoBehaviour
{
    public event Action OnComplete = delegate { };

    [SerializeField] Vector2 pushRange;

    public Vector3 localAxis;
    public Transform visualTarget;
    public float resetspeed = 5;

    private XRBaseInteractable xrBaseInteractable;
    private Vector3 offset;
    private Transform pokeAttachTransform;
    private Vector3 initialLocalpos;

    float threshold;
    float axisVal;

    float thresholdPercentage = 0.2f; // When the button goes below this, it turns red and activates whatever function

    [SerializeField, Tooltip("Set to true if the range is from a -ve value to 0. False if its from 0 to a +ve value")]
    bool inwardButton = true;

    [Header("DEBUGGING")]
    [SerializeField] bool buttonIsPushedDown = false;
    [SerializeField] ButtonPressState buttonPressState = ButtonPressState.Neutral;

    Interactable interactable;
    bool isEnabled;

    enum Direction
    {
        X,
        Y,
        Z
    }

    enum ButtonPressState
    {
        PressingDown,
        GoingUp,
        Neutral
    }

    Direction dir;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.OnEnable += EnableSelf;
    }
    
    void Start()
    {
        initialLocalpos = visualTarget.localPosition;
        xrBaseInteractable = GetComponent<XRBaseInteractable>();

        xrBaseInteractable.hoverEntered.AddListener(Follow);
        xrBaseInteractable.hoverExited.AddListener(ResetButtonPosition);

        //originalMaterial = buttonRenderer.material;
        float thresholdPercent = inwardButton ? thresholdPercentage : 1 - thresholdPercentage;

        threshold = (thresholdPercent * (pushRange.y - pushRange.x)) + pushRange.x;

        if (Mathf.Abs(localAxis.x) > 0)
            dir = Direction.X;
        else if (Mathf.Abs(localAxis.y) > 0)
            dir = Direction.Y;
        else
            dir = Direction.Z;
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            buttonPressState = ButtonPressState.PressingDown;

            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    public void EnableSelf() => isEnabled = true;

    public void ResetButtonPosition(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            buttonPressState = ButtonPressState.GoingUp;
            buttonIsPushedDown = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;

        switch (buttonPressState)
        {
            case ButtonPressState.PressingDown:
                Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
                Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
                Vector3 constrainedWorldPos = visualTarget.TransformPoint(constrainedLocalTargetPosition);

                Vector3 posWithRespectToParent = transform.parent.InverseTransformPoint(constrainedWorldPos);

                Vector3 clampedPosWRTParent = Vector3.zero;

                if (dir == Direction.X)
                {
                    axisVal = Mathf.Clamp(posWithRespectToParent.x, pushRange.x, pushRange.y);
                    clampedPosWRTParent = new Vector3(axisVal, posWithRespectToParent.y, posWithRespectToParent.z);
                }
                else if (dir == Direction.Y)
                {
                    axisVal = Mathf.Clamp(posWithRespectToParent.y, pushRange.x, pushRange.y);
                    clampedPosWRTParent = new Vector3(posWithRespectToParent.x, axisVal, posWithRespectToParent.z);
                }
                else
                {
                    axisVal = Mathf.Clamp(posWithRespectToParent.z, pushRange.x, pushRange.y);
                    clampedPosWRTParent = new Vector3(posWithRespectToParent.x, posWithRespectToParent.y, axisVal);
                }

                Vector3 clampedPosGlobal = transform.parent.TransformPoint(clampedPosWRTParent);

                visualTarget.position = clampedPosGlobal;
                break;

            case ButtonPressState.GoingUp:
                visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalpos, Time.deltaTime * resetspeed);
                if (Vector3.Distance(visualTarget.localPosition, initialLocalpos) < (0.1f * Mathf.Abs(pushRange.y - pushRange.x)))
                {
                    visualTarget.localPosition = initialLocalpos;
                    buttonPressState = ButtonPressState.Neutral;
                }
                break;
        }

        // Change button colour
        if (!buttonIsPushedDown && buttonPressState == ButtonPressState.PressingDown && IsButtonPressedDown())
        {
            ButtonPressed();
            buttonIsPushedDown = true;
        }
    }

    void ButtonPressed()
    {
        interactable.ButtonPressed();
    }

    bool IsButtonPressedDown() => inwardButton ? axisVal <= threshold : axisVal >= threshold;
}
