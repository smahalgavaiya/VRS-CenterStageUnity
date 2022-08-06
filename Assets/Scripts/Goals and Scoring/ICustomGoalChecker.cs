using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomGoalChecker
{
    public void DoCustomCheck();
    public void DoCustomCheck(GameObject objectToCheck);
}
