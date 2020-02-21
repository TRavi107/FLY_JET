using UnityEngine;

[CreateAssetMenu(menuName = "NinjaBoyAI/Actions/Chase")]
public class ChaseAction : Actions
{
    public override void Act(NinjaBoyController controller)
    {
        controller.Chase();

    }
}