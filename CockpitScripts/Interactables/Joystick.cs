using Unity.VisualScripting;
using UnityEngine;

public class Joystick : MonoBehaviour
{

    [SerializeField] Transform joystickTransform;
    [SerializeField] Transform targetTransform;
    [SerializeField] float range = 0.3f;
    [SerializeField] Vector2 handleRotXRange;
    [SerializeField] Vector2 handleRotZRange;

    float rotXDist;
    float rotZDist;

    float targetLocalYPos;

    [Header("DEBUGGING")]
    [SerializeField] bool useDebuggingInput;
    [SerializeField, Range(-1, 1)] float debugginInputX;
    [SerializeField, Range(-1, 1)] float debuggingInputY;

    private void Start()
    {
        targetLocalYPos = 0.5f;
        targetTransform.localPosition = Vector3.up * targetLocalYPos;

        rotXDist = handleRotXRange.y - handleRotXRange.x;
        rotZDist = handleRotZRange.y - handleRotZRange.x;
    }

    public void SetJoystickVal(float inputX, float inputY)
    {
        if (useDebuggingInput)
        {
            SetTargetPos(debugginInputX, debuggingInputY);
        } else
        {
            SetTargetPos(inputX, inputY);
        }

        FaceTarget();
    }

    void SetTargetPos(float inputX, float inputY)
    {
        Vector2 input = new Vector2(inputX, inputY);
        Vector2 clampedInput = Vector2.ClampMagnitude(input, 1.0f);  // Clamp input to max magnitude of 1
        Vector2 targetVector = clampedInput.normalized * clampedInput.magnitude * range;  // Scale both axes uniformly
        targetTransform.localPosition = new Vector3(targetVector.x, targetLocalYPos, targetVector.y);
    }

    void SetRotDirectly(float inputX, float inputY)
    {
        Vector2 input = new Vector2(inputX, inputY);
        //Vector2 clampedInput = Vector2.ClampMagnitude(input, 1.0f);  // Clamp input to max magnitude of 1
        //Vector2 targetVector = clampedInput.normalized * clampedInput.magnitude * range;  // Scale both axes uniformly
        input = input.normalized;

        float xAngle = handleRotXRange.x + (input.x * rotXDist);
        float zAngle = handleRotZRange.x + (input.y * rotZDist);

        joystickTransform.localEulerAngles = new Vector3(xAngle, 0, -zAngle);

        //joystickTransform.localEulerAngles = new Vector3(targetVector.x, 0, targetVector.y);
    }

    void FaceTarget()
    {
        joystickTransform.LookAt(targetTransform.position);
    }

    private void OnValidate()
    {
        if (useDebuggingInput)
        {
            SetTargetPos(debugginInputX, debuggingInputY);
            FaceTarget();
        }
    }
}
