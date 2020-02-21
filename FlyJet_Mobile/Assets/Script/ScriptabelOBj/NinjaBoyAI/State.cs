using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="NinjaBoy/State")]
public class State : ScriptableObject
{
    public Actions[] actions;
    public DecisionTransition[] decisions;

    public void Execute(NinjaBoyController controller)
    {
        PerformAction(controller);
        PerformDecision(controller);
    }

    public void OnStateStart(NinjaBoyController controller)
    {

    }
    public void OnStateEnd(NinjaBoyController controller)
    {

    }

    private void PerformDecision(NinjaBoyController controller)
    {
        for (int i = 0; i < decisions.Length; i++)
        {
            bool decision = decisions[i].decision.Decide(controller);
            if (decision)
            {
                controller.ChangeState(decisions[i].trueState);
            }
            else
            {
                controller.ChangeState(decisions[i].falseState);
            }
        }
    }

    private void PerformAction(NinjaBoyController controller)
    {

        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }
}
