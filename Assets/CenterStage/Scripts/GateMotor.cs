using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMotor : MonoBehaviour
{
    private HingeJoint[] joints;
    // Start is called before the first frame update
    void Start()
    {
        joints = GetComponents<HingeJoint>();
    }

    public void ToggleMotor(bool enable = true)
    {
        if(PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            StartCoroutine(Toggle(enable));
        }
    }

    IEnumerator Toggle(bool enable)
    {
        //Weirdly the springs arent as strong if they are enabled at the same time??? so have to do individually.
        joints[0].useSpring = enable;
        yield return new WaitForSeconds(0.01f);
        joints[1].useSpring = enable;
        yield return null;
    }
}
