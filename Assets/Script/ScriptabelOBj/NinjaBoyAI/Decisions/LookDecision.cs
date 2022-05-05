using UnityEngine;

[CreateAssetMenu(menuName ="Decision/LookDecision")]
public class LookDecision : Decision
{
    public State chaseState;
    public override bool Decide(NinjaBoyController controller)
    {
        RaycastHit2D hit = Physics2D.Raycast(controller.eyePos.position, controller.transform.right, 20);
        Debug.DrawRay(controller.eyePos.position, controller.transform.right * 20,Color.green);
        if(hit.collider!= null)
        {
            if (hit.collider.transform.parent == controller.transform.parent)
            {
                controller.ChangeState(chaseState);
                controller.destination = hit.transform;
                return false;
            }
            if (hit.collider.gameObject.CompareTag("Hero"))
            {
                return true;
            }

        }
        return false;
    }
}
