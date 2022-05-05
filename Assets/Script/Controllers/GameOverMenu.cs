using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI coinsCount;
    public TextMeshProUGUI killsCount;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI HighScoreMessage;
    public TextMeshProUGUI CongratsMessage;

    public GameStats temposcore;

    void Start()
    {
        AudioManager.instance.Stop("HappyTune");
        AudioManager.instance.Play("benSound");
        coinsCount.text = temposcore.coinsCount.ToString();
        killsCount.text = temposcore.killsCount.ToString();
        Score.text = temposcore.score.ToString();
        PlayerData highscore = FJ.SaveSystem.LoadScore();

        if (highscore ==null || temposcore.score > highscore.highScore)
        {
            FJ.SaveSystem.SaveHighScore(temposcore);
            HighScore.gameObject.SetActive(false);
            HighScoreMessage.gameObject.SetActive(false);
            CongratsMessage.gameObject.SetActive(true);
        }
        else
        {
            HighScore.gameObject.SetActive(true);
            HighScore.text = highscore.highScore.ToString();
            HighScoreMessage.gameObject.SetActive(true);
            CongratsMessage.gameObject.SetActive(false);

        }
    }

    public void RePlay()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
