using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score;
    [SerializeField]
    private Text scoreCounter;

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
        scoreCounter.text = Score.ToString();
    }
}
