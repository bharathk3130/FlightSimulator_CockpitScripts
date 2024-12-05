using UnityEngine;


[RequireComponent(typeof(Interactable))]
public class Lever : MonoBehaviour
{
    Interactable interactable;

    [SerializeField] float[] targetPercentages; // Percentage the lever should go to first
    [SerializeField] float errorAccepted = 0.05f;
    [SerializeField] bool isYoke = false;
    int currentTarget = 0;

    [SerializeField] LeverType leverType;

    RigidbodyConstraints defaultConstraints;

    Rigidbody rb;

    Quaternion[] flapsRange = new Quaternion[] { new Quaternion(-0.65723f, -0.65723f, 0.26087f, -0.26087f),
                            new Quaternion(-0.45191f, -0.45191f, -0.54386f, 0.54386f)};
    Quaternion[] throttleRange = new Quaternion[] { new Quaternion(-0.98859f, 0.00000f, 0, 0.15066f),
                            new Quaternion(-0.70711f, 0, 0, 0.70711f)};
    Quaternion[] landingGearRange = new Quaternion[] { new Quaternion(-0.95188f, 0, 0, -0.30646f),
                            new Quaternion(-0.96102f, 0, 0, 0.27648f)};

    // Values have not been set up
    Quaternion[] controllerRange = new Quaternion[] { new Quaternion(-0.65723f, -0.65723f, 0.26087f, -0.26087f),
                            new Quaternion(-0.45191f, -0.45191f, -0.54386f, 0.54386f)};
    Quaternion[] controllerSupportRange = new Quaternion[] { new Quaternion(-0.65723f, -0.65723f, 0.26087f, -0.26087f),
                            new Quaternion(-0.45191f, -0.45191f, -0.54386f, 0.54386f)};

    bool isEnabled;

    enum LeverType
    {
        Flaps,
        Throttle,
        LandingGear,
        Controller,
        ControllerSupport
    }

    void Awake()
    {
        interactable = GetComponent<Interactable>();
        rb = transform.GetComponent<Rigidbody>();
        interactable.OnEnable += EnableSelf;

        defaultConstraints = rb.constraints;

        if (!isEnabled && !isYoke)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    [ContextMenu("Enable Lever")]
    void EnableSelf()
    {
        rb.constraints = defaultConstraints;
        isEnabled = true;
    }

    void Update()
    {
        if (!isEnabled || currentTarget >= targetPercentages.Length) return;

        if (ReachedTarget(targetPercentages[currentTarget]))
        {
            ButtonPressed();
            currentTarget++;
        }
    }

    bool ReachedTarget(float target) => Mathf.Abs(CalculateLeverPercentage() - target) <= errorAccepted;

    void PrintLeverPercentage() => print(CalculateLeverPercentage());
    void PrintRotation() => print(transform.rotation);

    float CalculateLeverPercentage()
    {
        // Calculate the angles between start and end, and start and target
        float angleStartToEnd = Quaternion.Angle(GetRotationRange()[0], GetRotationRange()[1]);
        float angleStartToTarget = Quaternion.Angle(GetRotationRange()[0], transform.rotation);

        // Calculate the percentage
        float percentage = angleStartToTarget / angleStartToEnd;

        // Clamp the percentage to be between 0 and 1
        percentage = Mathf.Clamp01(percentage);

        return percentage;
    }

    void ButtonPressed()
    {
        interactable.ButtonPressed();
    }

    Quaternion[] GetRotationRange()
    {
        switch (leverType)
        {
            case LeverType.Flaps:
                return flapsRange;
            case LeverType.Throttle:
                return throttleRange;
            case LeverType.LandingGear:
                return landingGearRange;
            case LeverType.Controller:
                return controllerRange;
            case LeverType.ControllerSupport:   
                return controllerSupportRange;
            default:
                return null;
        }
    }
}
