using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecisionTransition 
{
    public Decision decision;
    public State trueState;
    public State falseState;
}
