using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FusionRobotInit : NetworkBehaviour
{
    void Start()
    {
        if(Object.HasInputAuthority)
            RPC_SetDrives();
    }

    [Rpc(RpcSources.InputAuthority,RpcTargets.InputAuthority)]
    public void RPC_SetDrives()
    {
            Debug.Log("RPC Called");
            DriveReceiverSpinningWheels dr = gameObject.GetComponent<DriveReceiverSpinningWheels>();

            Drive frontRight = Instantiate(dr.frontRight);
            frontRight.RegisterDriveReceiver(dr);
            Drive frontLeft = Instantiate(dr.frontLeft);
            frontLeft.RegisterDriveReceiver(dr);
            Drive backRight = Instantiate(dr.backRight);
            backRight.RegisterDriveReceiver(dr);
            Drive backLeft = Instantiate(dr.backLeft);
            backLeft.RegisterDriveReceiver(dr);

            dr.frontRight = frontRight;
            dr.frontLeft = frontLeft;
            dr.backRight = backRight;
            dr.backLeft = backLeft;
            dr.UpdateDrivers();

            InputActionManager IAM = GameObject.FindObjectOfType<InputActionManager>();
            IAM.frontRightWheel = frontRight;
            IAM.frontLeftWheel = frontLeft;
            IAM.backRightWheel = backRight;
            IAM.backLeftWheel = backLeft;
            IAM.robot = gameObject;
            IAM.UpdateAllDrives();

    }

}
