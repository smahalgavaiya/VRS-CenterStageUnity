using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateWheelColliders : EditorWindow
{
    [SerializeField]
    private GameObject robotParent;
    GameObject wheelsParent;
    private GameObject wheelCollidersParent;

    public void CreatePopup()
    {
        EditorWindow window = EditorWindow.CreateInstance<GenerateWheelColliders>();
        EditorStyles.label.wordWrap = true;
        window.Show();
    }

    private void OnGUI()
    {
        robotParent = (GameObject)EditorGUI.ObjectField(new Rect(3, 5, position.width - 6, 20), "Robot Parent", robotParent, typeof(GameObject), true);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Wheel Colliders"))
        {
            if (robotParent == null)
            {
                ShowNotification(new GUIContent("You need to select \n the Robot Parent (above) \n for the root of the robot"), 5);
                return;
            }
            List<GameObject> wheels = new List<GameObject>();

            bool wheelsParentCreated = false;

            wheelsParentCreated = CheckWheelsParentExists(wheelsParentCreated);

            if (!wheelsParentCreated)
            {
                wheelsParent = new GameObject("WheelModels");
                wheelsParent.transform.SetParent(robotParent.transform);
                wheelsParent.transform.rotation = robotParent.transform.rotation;

                wheelCollidersParent = new GameObject("WheelColliders");
                wheelCollidersParent.transform.SetParent(robotParent.transform);
                wheelCollidersParent.transform.rotation = robotParent.transform.rotation;
            }

            MoveWheelsToWheelsParent();
        }
    }

    private void MoveWheelsToWheelsParent()
    {
        for (int i = 0; i < robotParent.transform.childCount; i++)
        {
            GameObject robotPart = robotParent.transform.GetChild(i).gameObject;
            if (
                (robotPart.name.Contains("wheel") || robotPart.name.Contains("Wheel"))
                && !robotPart.name.Contains("Colliders") 
                && !robotPart.name.Contains("Models")
               )
            {
                GameObject newWheel = Instantiate(robotPart);
                GameObject newWheelCollider = Instantiate(robotPart);

                MatchLocRotScale(robotPart, newWheel);
                MatchLocRotScale(robotPart, newWheelCollider);

                newWheel.AddComponent<DriveReceiverForTransformRotate>();
                WheelCollider wheelCollider = newWheelCollider.AddComponent<WheelCollider>();
                newWheelCollider.GetComponent<Renderer>().enabled = false;

                newWheel.transform.SetParent(wheelsParent.transform);
                newWheelCollider.transform.SetParent(wheelCollidersParent.transform);
                //robotPart.SetActive(false);
            }
        }
    }

    private static void MatchLocRotScale(GameObject robotPart, GameObject newWheel)
    {
        newWheel.transform.position = robotPart.transform.position;
        newWheel.transform.rotation = robotPart.transform.rotation;
        newWheel.transform.localScale = robotPart.transform.localScale;
    }

    private bool CheckWheelsParentExists(bool wheelsParentCreated)
    {
        for (int i = 0; i < robotParent.transform.childCount; i++)
        {
            GameObject robotPart = robotParent.transform.GetChild(i).gameObject;
            if (robotPart.name == "WheelsModels")
            {
                wheelsParentCreated = true;
            } else wheelsParentCreated = false;

        }

        return wheelsParentCreated;
    }
}
