using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stats/GameStats")]
public class GameStats : ScriptableObject
{
    public int coinsCount;
    public int killsCount;
    public int score;
}
