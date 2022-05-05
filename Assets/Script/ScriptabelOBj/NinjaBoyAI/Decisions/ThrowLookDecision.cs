using UnityEngine;

[CreateAssetMenu(menuName = "Decision/ThrowLookDecision")]
public class ThrowLookDecision : Decision
{
    public float notSeenTime;
    public State chaseState;
    public override bool Decide(NinjaBoyController controller)
    {
        RaycastHit2D hit = Physics2D.Raycast(controller.eyePos.position, controller.transform.right, 20);
        Debug.DrawRay(controller.eyePos.position, controller.transform.right * 20, Color.green);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Hero"))
        {
            if (hit.collider.transform.parent == controller.transform.parent)
            {
                controller.ChangeState(chaseState);
                controller.destination = hit.transform;
                return true;
            }
            notSeenTime = 0;
            if (controller.now - controller.lastThrown > controller.throwInterval)
            {
                controller.lastThrown = controller.now;
                controller.myanimator.SetTrigger("throw");
            }
            return true;
        }
        if(hit.collider == null || !hit.collider.gameObject.CompareTag("Hero"))
        {
            notSeenTime += Time.deltaTime;
            if (notSeenTime > 2)
            {
                notSeenTime = 0;
                return false;
            }
        }
        return true;

    }
}
