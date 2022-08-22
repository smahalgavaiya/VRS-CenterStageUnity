using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotManagerVR : MonoBehaviour
{
    [SerializeField]
    public List<DriveReceiverForVirtualMecanumWheel> wheels = new List<DriveReceiverForVirtualMecanumWheel>();

    //Moves Joints and Robot
    private void Update()
    {
        //if (UIBehavior.currScene == UIBehavior.sceneState.DriverControl || UIBehavior.currScene == UIBehavior.sceneState.ConfigTest)
        {
            //Temporary Controls Setup
            if (wheels.Count < 4)
                return;
            float forward = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
            float strafe = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
            float rotate = (Keyboard.current.eKey.isPressed ? 1 : 0) - (Keyboard.current.qKey.isPressed ? 1 : 0);
            if (Gamepad.current != null)
            {
                forward += Gamepad.current.leftStick.y.ReadValue();
                strafe += Gamepad.current.leftStick.x.ReadValue();
                rotate += Gamepad.current.rightStick.x.ReadValue();
            }
            wheels[0].setPower(forward + strafe + rotate); //frontLeft
            wheels[1].setPower(-(forward - strafe - rotate)); //frontRight
            wheels[2].setPower(forward - strafe + rotate); //backLeft
            wheels[3].setPower(-(forward + strafe - rotate)); //backRight

        }
    }

}
