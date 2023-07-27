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
        StartCoroutine(Toggle());
    }

    public void ToggleMotor(bool enable = true)
    {
        //joints[1].useSpring = enable;
        foreach(HingeJoint j in joints)
        {
            j.useSpring = enable;
        }
    }

    IEnumerator Toggle()
    {
        yield return new WaitForSeconds(10);
        //Weirdly the springs arent as strong if they are enabled at the same time??? so have to do individually.
        joints[0].useSpring = true;
        yield return new WaitForSeconds(0.01f);
        joints[1].useSpring = true;
        yield return new WaitForSeconds(2);
        ToggleMotor(false);
        yield return null;
    }
}
