using UnityEngine;

[CreateAssetMenu(menuName = "Decision/ChaseDecision")]
public class ChaseDecision : Decision
{
    public float notSeenTime;
    public override bool Decide(NinjaBoyController controller)
    {
        RaycastHit2D hit = Physics2D.Raycast(controller.eyePos.position, controller.transform.right, 20);
        Debug.DrawRay(controller.eyePos.position, controller.transform.right * 20, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Hero"))
            {
                notSeenTime = 0;
                controller.continueChase = true;
                return true;
            }

        }
        if (hit.collider == null || !hit.collider.gameObject.CompareTag("Hero"))
        {
            controller.continueChase = false;
            controller.myanimator.SetBool("attack", false);
            controller.myanimator.SetBool("run", false);
            notSeenTime += Time.deltaTime;
            if (notSeenTime > 2)
            {
                notSeenTime = 0;
                controller.destination = controller.Limit[Random.Range(0, controller.Limit.Count)];
                return false;
            }

        }
        return true;
    }
}
