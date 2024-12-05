using System;
using System.Collections;
using UnityEngine;

public class PokeLever : MonoBehaviour
{

    [SerializeField] LeverType leverType;
    [SerializeField] float rotationSpeed = 1.0f;

    Interactable interactable;

    int targetRotIndex = 0;
    bool _isEnabled = false;

    Quaternion[] flapsTargetValues = new Quaternion[] { new Quaternion(-0.49859f, -0.49857f, -0.50143f, 0.50141f),
                            new Quaternion(-0.69944f, -0.69941f, -0.10397f, 0.10398f)};
    Quaternion[] throttleTargetValues = new Quaternion[] {
        new Quaternion(-0.72737f, -0.00002f, 0.00001f, 0.68625f),
        new Quaternion(-0.94135f, -0.00002f, 0, 0.33743f),
        new Quaternion(-0.99752f, -0.00003f, 0, 0.07044f)};
    Quaternion[] landingGearTargetValues = new Quaternion[] { new Quaternion(-0.98725f, -0.00006f, 0.00001f, 0.15916f),
                            new Quaternion(-0.98623f, -0.00002f, 0, -0.16535f)};

    Quaternion[] targetRotations;

    float[] throttleTargetPercentages = { 1, 0.75f, 0 };
    public Action OnLeverMove = delegate { };

    enum LeverType
    {
        Flaps,
        Throttle,
        LandingGear,
    }

    void Awake()
    {
        interactable = GetComponent<Interactable>();
        targetRotations = GetTargetRotations();
    }

    void OnEnable()
    {
        interactable.OnEnable += EnableSelf;
    }

    void OnDisable()
    {
        interactable.OnEnable -= EnableSelf;
    }

    [ContextMenu("Enable Lever")]
    void EnableSelf() => _isEnabled = true;

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Hand") && _isEnabled)
        {
            _isEnabled = false;
            StartCoroutine(LerpTo(targetRotations[targetRotIndex]));
            interactable.ButtonPressed();

            OnLeverMove();
            targetRotIndex++;
        }
    }

    IEnumerator LerpTo(Quaternion targetRotation)
    {
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }


    Quaternion[] GetTargetRotations()
    {
        switch (leverType)
        {
            case LeverType.Flaps:
                return flapsTargetValues;
            case LeverType.Throttle:
                return throttleTargetValues;
            case LeverType.LandingGear:
                return landingGearTargetValues;
            default:
                return null;
        }
    }

    // Used only on the throttle lever
    public float GetThrottlePercentage() => throttleTargetPercentages[targetRotIndex];

    [ContextMenu("Print rotation")]
    void PrintRotation() => print(transform.rotation);
}