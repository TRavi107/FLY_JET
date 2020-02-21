using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NinjaBoyAI/Actions/Throw")]
public class ThrowAction : Actions
{
    public override void Act(NinjaBoyController controller)
    {
        controller.StopCoroutine("LookAround");
        controller.looking = false;
        controller.myanimator.SetBool("run", false);
        controller.myanimator.SetBool("attack", false);
    }
}
