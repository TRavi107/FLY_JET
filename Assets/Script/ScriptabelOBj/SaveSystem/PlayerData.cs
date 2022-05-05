using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coinsCollected;
    public int killsCount;
    public int highScore;

    public PlayerData(GameStats gameStats)
    {
        coinsCollected = gameStats.coinsCount;
        killsCount = gameStats.killsCount;
        highScore = gameStats.score;
    }
}
