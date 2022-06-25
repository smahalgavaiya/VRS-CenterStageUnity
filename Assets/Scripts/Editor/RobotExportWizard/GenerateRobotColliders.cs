using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateRobotColliders : EditorWindow
{
    [SerializeField]
    private GameObject robotParent;
    GameObject wheelsParent;
    private GameObject wheelCollidersParent;

    [SerializeField]
    private bool hasMecanumWheels;

    public void CreatePopup()
    {
        EditorWindow window = EditorWindow.CreateInstance<GenerateRobotColliders>();
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

        hasMecanumWheels = (bool)EditorGUILayout.Toggle("Has Mecanum Wheels?", hasMecanumWheels);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Robot Colliders"))
        {
            if (robotParent == null)
            {
                ShowNotification(new GUIContent("You need to select \n the Robot Parent (above) \n for the root of the robot"), 5);
                return;
            }

            if (robotParent.GetComponent<Rigidbody>() == null)
            {
                Rigidbody robotRB = robotParent.AddComponent<Rigidbody>();
                robotRB.mass = 1000;
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

                MoveWheelsToWheelsParent();
            }

            if (wheelsParentCreated)
            {
                ShowNotification(new GUIContent("It looks like you \n already made wheel colliders \n " +
                    "if you want to restart \n delete the WheelColliders \n and WheelModels \n objects from " +
                    "your robot hierarchy."), 5);
                return;
            }

            if (hasMecanumWheels && robotParent.GetComponent<DriveReceiverMecanum>() == null)
            {
                robotParent.AddComponent<DriveReceiverMecanum>();
            }
        }
    }

    private void MoveWheelsToWheelsParent()
    {
        bool wheelsNamedCorrectly = false;

        for (int i = 0; i < robotParent.transform.childCount; i++)
        {
            GameObject robotPart = robotParent.transform.GetChild(i).gameObject;

            if (robotPart.GetComponent<MeshCollider>() == null)
            {
                MeshCollider meshCollider = robotPart.AddComponent<MeshCollider>();
                meshCollider.convex = true;
            }

            robotPart.layer = LayerMask.NameToLayer("Robot");

            if (
                (robotPart.name.Contains("wheel") || robotPart.name.Contains("Wheel"))
                && !robotPart.name.Contains("Colliders") 
                && !robotPart.name.Contains("Models")
               )
            {
                wheelsNamedCorrectly = true;

                // We do not want the wheels to have mesh colliders, as they use wheel colliders
                // however we globally added mesh colliders to every part in the for loop above,
                // so we need to now delete it.
                if (robotPart.GetComponent<MeshCollider>() != null)
                {
                    DestroyImmediate(robotPart.GetComponent<MeshCollider>());
                }

                GameObject newWheel = Instantiate(robotPart);
                GameObject newWheelCollider = Instantiate(robotPart);

                newWheel.name = newWheel.name.Remove(newWheel.name.IndexOf("("), 7);
                newWheelCollider.name = newWheel.name + "_WheelCollider";
                newWheel.name = newWheel.name + "_WheelModel";

                MatchLocRotScale(robotPart, newWheel);
                MatchLocRotScale(robotPart, newWheelCollider);

                newWheel.AddComponent<DriveReceiverForTransformRotate>();
                WheelCollider wheelCollider = newWheelCollider.AddComponent<WheelCollider>();
                wheelCollider.suspensionDistance = .1f;
                newWheelCollider.GetComponent<Renderer>().enabled = false;

                newWheel.transform.SetParent(wheelsParent.transform);
                newWheelCollider.transform.SetParent(wheelCollidersParent.transform);
                robotPart.SetActive(false);
            }
        }
        if (!wheelsNamedCorrectly)
        {
            ShowNotification(new GUIContent("You need to name \n the wheels with the word \n \"wheel\" in the object name"), 5);
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
            if (robotPart.name == "WheelModels")
            {
                wheelsParentCreated = true;
                break;
            } else wheelsParentCreated = false;
        }

        return wheelsParentCreated;
    }
}
