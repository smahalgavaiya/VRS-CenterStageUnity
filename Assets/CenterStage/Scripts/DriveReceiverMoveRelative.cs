using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveDir { right = 0, up = 1, forward = 2 };

public class DriveReceiverMoveRelative : DriveReceiver
{
    // Start is called before the first frame update
    public float movementScale = 0.01f;
    public MoveDir moveAxis = MoveDir.right;
    public float moveLimitMax;
    public float moveLimitMin;
    public bool AtMaxLimit { get { return limitMax; } }
    public bool AtMinLimit { get { return limitMin; } }

    public DriveReceiverForTransformMove attachedTo;

    private bool limitMax = false;
    private bool limitMin = false;

    private float moveAmt = 0;

    public UnityEvent<float> onLimitReached;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3();
        Vector3 oldPos = transform.position;
        switch (moveAxis)
        {
            case MoveDir.right:
                moveDir = transform.right * drive.driveAmount.x;
                break;
            case MoveDir.up:
                moveDir = transform.up * drive.driveAmount.x;
                break;
            case MoveDir.forward:
                moveDir = transform.forward * drive.driveAmount.x;
                break;
        }
        /*Debug.DrawRay(transform.position, transform.forward * .5f, Color.red);
        Debug.DrawRay(transform.position, transform.up * .5f, Color.green);
        Debug.DrawRay(transform.position, transform.right * .5f, Color.yellow);*/
        Vector3 newPos = oldPos + (moveDir * movementScale);
        limitMax = false;
        limitMin = false;
        if (attachedTo != null && (!attachedTo.AtMaxLimit && !attachedTo.AtMinLimit)) { return; }

        moveAmt += drive.driveAmount.x * movementScale;

        if (moveAmt >= moveLimitMax || moveAmt <= moveLimitMin)
        {
            // Debug.Log(newPos);
            if (moveAmt >= moveLimitMax) { limitMax = true; moveAmt = moveLimitMax; /*onLimitReached.Invoke();*/ }
            else { limitMin = true; moveAmt = moveLimitMin; onLimitReached.Invoke(movementScale); onLimitReached.RemoveAllListeners(); }
            return;
        }


        transform.position = newPos;
    }
}
