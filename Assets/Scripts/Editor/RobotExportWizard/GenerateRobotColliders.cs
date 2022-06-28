using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GenerateRobotColliders : EditorWindow
{
    [SerializeField]
    private GameObject robotParent;
    GameObject wheelsParent;
    private GameObject wheelCollidersParent;

    [SerializeField]
    private bool hasMecanumWheels;

    DriveIndex driveIndex;

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
            driveIndex = GameObject.Find("@RobotExportManager").GetComponent<DriveIndex>();

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
                wheelsParent = robotParent.transform.Find("WheelModels").gameObject;
                wheelCollidersParent = robotParent.transform.Find("WheelColliders").gameObject;

                if (wheelsParent.transform.childCount < 1 || wheelCollidersParent.transform.childCount < 1)
                    MoveWheelsToWheelsParent();
                else
                    ShowNotification(new GUIContent("It looks like you already \n have wheel models and " +
                        "\n wheel colliders. Delete them \n if you'd like to start over."), 5);
            }

            if (hasMecanumWheels)
            {
                DriveReceiverMecanum driveReceiverMecanum;
                if (robotParent.GetComponent<DriveReceiverMecanum>() == null)
                    driveReceiverMecanum = robotParent.AddComponent<DriveReceiverMecanum>();
                else
                    driveReceiverMecanum = robotParent.GetComponent<DriveReceiverMecanum>();

                driveReceiverMecanum.FrontLeft = driveIndex.frontLeftWheel;
                driveReceiverMecanum.FrontRight = driveIndex.frontRightWheel;
                driveReceiverMecanum.BackLeft = driveIndex.backLeftWheel;
                driveReceiverMecanum.BackRight = driveIndex.backRightWheel;
            }
        }
    }

    private void MoveWheelsToWheelsParent()
    {
        for (int i = 0; i < robotParent.transform.childCount; i++)
        {
            GameObject robotPart = robotParent.transform.GetChild(i).gameObject;

            if (robotPart.GetComponent<MeshCollider>() == null && robotPart.GetComponent<MeshFilter>() != null)
            {
                MeshCollider meshCollider = robotPart.AddComponent<MeshCollider>();
                meshCollider.convex = true;
            }

            robotPart.layer = LayerMask.NameToLayer("Robot");

            if (!CheckWheelNaming())
            {
                ShowNotification(new GUIContent("You need to name \n the wheels using the convention \n \"frontLeftWheel\", \"backRightWheel\"etc."), 5);
                return;
            }

            if (
                robotPart.name.ToLower().Contains("wheel") 
                && !robotPart.name.Contains("Colliders") 
                && !robotPart.name.Contains("Models")
               )
            {
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
                WheelTurner wheelTurner = newWheel.AddComponent<WheelTurner>();
                AddCorrespondingDrive(newWheel);

                WheelCollider wheelCollider = newWheelCollider.AddComponent<WheelCollider>();

                wheelTurner.WheelCollider = wheelCollider;

                wheelCollider.suspensionDistance = .1f;
                newWheelCollider.GetComponent<Renderer>().enabled = false;

                newWheel.transform.SetParent(wheelsParent.transform);
                newWheelCollider.transform.SetParent(wheelCollidersParent.transform);
                robotPart.SetActive(false);
            }
        }

    }

    private void AddCorrespondingDrive(GameObject newWheel)
    {
        if (newWheel.name.ToLower().Contains("frontleft"))
        {
            newWheel.GetComponent<DriveReceiverForTransformRotate>().drive = driveIndex.frontLeftWheel;
        }
        else if (newWheel.name.ToLower().Contains("frontright"))
        {
            newWheel.GetComponent<DriveReceiverForTransformRotate>().drive = driveIndex.frontRightWheel;
        }
        else if (newWheel.name.ToLower().Contains("backleft"))
        {
            newWheel.GetComponent<DriveReceiverForTransformRotate>().drive = driveIndex.backLeftWheel;
        }
        else if (newWheel.name.ToLower().Contains("backright"))
        {
            newWheel.GetComponent<DriveReceiverForTransformRotate>().drive = driveIndex.backRightWheel;
        }
    }

    private bool CheckWheelNaming()
    {
        bool wheelsNamedCorrectly = false;
        int numberOfCorrectlyNamedWheels = 0;

        for (int i = 0; i < robotParent.transform.childCount; i++)
        {
            string partName = robotParent.transform.GetChild(i).name;
            if (
                (
                partName.ToLower().Contains("frontleft") || 
                partName.ToLower().Contains("frontright") || 
                partName.ToLower().Contains("backleft") || 
                partName.ToLower().Contains("backright") 
                ) && 
                (partName.ToLower().Contains("wheel"))
               )
            {
                numberOfCorrectlyNamedWheels++;
            }
        }

        if (numberOfCorrectlyNamedWheels == 4)
            wheelsNamedCorrectly = true;
        else wheelsNamedCorrectly = false;
        
        return wheelsNamedCorrectly;
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
