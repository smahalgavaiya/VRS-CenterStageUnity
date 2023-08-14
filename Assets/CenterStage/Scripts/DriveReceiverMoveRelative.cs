using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveDir { right = 0, up = 1, forward = 2 };

public class DriveReceiverMoveRelative : DriveReceiver
{
    // Start is called before the first frame update
    public float movementScalePos = 0.01f;
    public float movementScaleNeg = 0.0001f;
    public MoveDir moveAxis = MoveDir.right;
    public float moveLimitMin;
    public bool AtMaxLimit { get { return limitMax; } }
    public bool AtMinLimit { get { return limitMin; } }

    public DriveReceiverForTransformMove attachedTo;

    private bool limitMax = false;
    private bool limitMin = false;

    private float moveAmt = 0;

    public float MoveAmount { get { return moveAmt; } }
    public float Distance { get; set; }

    public UnityEvent<float> onLimitReached;

    private float maxExtReached = 0;

    public Transform maxLimitPos;
    public Transform minLimitPos;

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveDir = new Vector3();
        Vector3 oldPos = transform.position;
        float oldPosition = 0;
        Vector3 moveLimitMaxPos = transform.position;
        Vector3 moveLimitMinPos = transform.position;
        switch (moveAxis)
        {
            case MoveDir.right:
                moveDir = transform.right * drive.driveAmount.x;
                moveLimitMinPos.x = moveLimitMin;
                oldPosition = transform.position.x;
                break;
            case MoveDir.up:
                moveDir = transform.up * drive.driveAmount.x;
                moveLimitMinPos.y = moveLimitMin;
                oldPosition = transform.position.y;
                break;
            case MoveDir.forward:
                moveDir = transform.forward * drive.driveAmount.x;
                moveLimitMinPos.z = moveLimitMin;
                oldPosition = transform.position.z;
                break;
        }
        float movementScale = movementScaleNeg;
        if (drive.driveAmount.x > 0)
        {
            movementScale = movementScalePos;
        }
        Vector3 newPos = oldPos + (moveDir * movementScale);
        if (drive.driveAmount.x > 0) 
        { 
            if (Vector3.Distance(oldPos, maxLimitPos.position) < 0.05)
            {
                transform.position = maxLimitPos.position;
                Debug.Log($"maxExt{maxExtReached}, moveScale: {movementScale}");
                onLimitReached.Invoke((maxExtReached) * (movementScale * 4));
                return; 
            }
        }
        else
        {
            if (Vector3.Distance(oldPos, minLimitPos.position) < 0.05)
            { return; }
        }

        
        limitMax = false;
        limitMin = false;
        if (attachedTo != null && (!attachedTo.AtMaxLimit && !attachedTo.AtMinLimit)) { return; }

        moveAmt = oldPosition + (drive.driveAmount.x * movementScale);

        Distance = Vector3.Distance(transform.position, maxLimitPos.position);
        //if ((float)moveAmt - moveLimitMin > maxExtReached) { maxExtReached = (float)moveAmt - moveLimitMin; }
        if(Distance > maxExtReached) { maxExtReached = Distance; }

        transform.position = newPos;
    }
}
