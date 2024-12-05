using UnityEngine;
using TMPro;

public class PlaneOrientationUI : MonoBehaviour
{
    [SerializeField] PlaneOrientation planeOrientation;
    [SerializeField] TextMeshProUGUI pitchRollYawText;
    [SerializeField] AirplaneController airplaneController;
    [SerializeField] Rigidbody planeRb;

    void Update()
    {
        // Update the text with pitch, roll, and yaw values
        UpdatePitchRollYawUI();
    }

    private void UpdatePitchRollYawUI()
    {
        if (planeOrientation != null && pitchRollYawText != null)
        {
            string speed = Utility.mpsToKnots(airplaneController.GetVelocity()).ToString("0.00");
            string altitude = Weather.GetAltitude(planeRb.position.y).ToString("0.00");

            // Format the text to display the pitch, roll, and yaw on different lines
            pitchRollYawText.text = "Pitch: " + planeOrientation.pitch.ToString("F2") +
                                    "\nRoll: " + planeOrientation.roll.ToString("F2") +
                                    "\nYaw: " + planeOrientation.yaw.ToString("F2") +
                                    $"\n\nSpeed: {speed} knots" +
                                    $"\nAltitude: {altitude} m";
        }
    }
}
