using UnityEngine;

public class PlaneOrientation : MonoBehaviour
{
    // Variables to hold the calculated pitch, roll, and yaw
    public float pitch;
    public float roll;
    public float yaw;

    void Update()
    {
        // Calculate the pitch, roll, and yaw relative to the plane's local axes
        CalculatePitchRollYaw();
    }

    private void CalculatePitchRollYaw()
    {
        // Get the forward, right, and up directions of the plane
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        // Pitch (rotation around the right axis, X axis of the local space)
        pitch = Mathf.Atan2(forward.y, forward.z) * Mathf.Rad2Deg;

        // Roll (rotation around the forward axis, Z axis of the local space)
        roll = Mathf.Atan2(right.y, right.x) * Mathf.Rad2Deg;

        // Yaw (rotation around the up axis, Y axis of the local space)
        yaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
    }
}
