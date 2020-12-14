using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score;
    [SerializeField]
    private Text scoreCounter;
    [SerializeField]
    private Text highScoreCounter;
    private int HighScore;

    void Awake()
    {
        SetupNewGame();
    }

    private static void SetupNewGame()
    {
        Score = 0;
    }

    private void Update()
    {
        if(HighScore<Score)
        {
            HighScore = Score;
            highScoreCounter.text = HighScore.ToString();
        }
        scoreCounter.text = Score.ToString();
    }
}
