using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal interface ICustomGoalEvents
{
    public void DoCustomOnEvent(Object objectToPass);
    public void DoCustomOffEvent(Object objectToPass);
}