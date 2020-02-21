using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="NinjaBoyAI/Actions/Patrol")]
public class PatrolAction : Actions
{
    public override void Act(NinjaBoyController controller)
    {
        controller.Patrol();
    }
}
