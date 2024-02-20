using UnityEngine;
using TMPro;

public class DisplayAcceleration : MonoBehaviour
{
    public static DisplayAcceleration instance; // Static reference to the instance of DisplayAcceleration
    private TextMeshProUGUI accelerationText; // Reference to the TextMeshPro component

    private void Awake()
    {
        // Ensure only one instance of DisplayAcceleration exists
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Get the TextMeshPro component attached to the same GameObject
        accelerationText = GetComponent<TextMeshProUGUI>();
        FindAnyObjectByType<IMUSensor>();
    }

    void FixedUpdate()
    {
        // Find any object in the scene that has an IMUSensor component attached
        IMUSensor imuSensor = FindObjectOfType<IMUSensor>();

        if (imuSensor != null)
        {
            // Access the Accelerations property of the IMUSensor component
            Vector3 acceleration = imuSensor.Accelerations;

            // Call the SetText function of DisplayAcceleration instance (assuming DisplayAcceleration is a singleton)
            SetText(acceleration);
        }
        else
        {
            Debug.LogWarning("IMUSensor not found in the scene.");
        }
    }



    // Function to set the text of the TextMeshPro component
    public void SetText(Vector3 acceleration)
    {
        // Set the text to display acceleration values
        accelerationText.SetText("Acceleration x: " + (int)acceleration.x + "\n y: " + (int)acceleration.y + "\n z: " + (int)acceleration.z);
    }
}
