using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] difficultyButtons;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private WallManager wallManager;
    [SerializeField]
    private Arena arena;

    public enum GameState { menu,play,pause};
    public static GameState gameState = GameState.menu;
    public enum Difficulty { easy, medium, hard };
    public static Difficulty difficulty = Difficulty.easy;

    private void Start()
    {
        Time.timeScale = 0;
        //When game starts, only easy button is shown
        for (int x=1;x<difficultyButtons.Length;x++)
        {
            difficultyButtons[x].SetActive(false);
        }
        wallManager.ChangeDifficulty();
    }

    //Function to be called by menu buttons to change difficulty
    public void ChangeDifficulty()
    {
        foreach (GameObject button in difficultyButtons)
        {
            button.SetActive(false);
        }
        switch (difficulty)
        {
            case Difficulty.easy:
                difficulty = Difficulty.medium;
                difficultyButtons[1].SetActive(true);
                break;
            case Difficulty.medium:
                difficulty = Difficulty.hard;
                difficultyButtons[2].SetActive(true);
                break;
            case Difficulty.hard:
                difficulty = Difficulty.easy;
                difficultyButtons[0].SetActive(true);
                break;
        }
        wallManager.ChangeDifficulty();
        arena.ChangeDifficulty();
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        gameState = GameState.pause;
        menuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        gameState = GameState.menu;
        menuPanel.SetActive(true);
        switch (difficulty)
        {
            case Difficulty.easy:
                difficultyButtons[0].SetActive(true);
                break;
            case Difficulty.medium:
                difficultyButtons[1].SetActive(true);
                break;
            case Difficulty.hard:
                difficultyButtons[2].SetActive(true);
                break;
        }
    }

    public void PlayGame()
    {
        menuPanel.SetActive(false);
        if (gameState==GameState.menu)
        {
            foreach (GameObject button in difficultyButtons)
            {
                button.SetActive(false);
            }
        }
        else if(gameState==GameState.pause)
        {

        }
        wallManager.gameObject.SetActive(true);
        wallManager.StartSpawn();
        gameState = GameState.play;
        Time.timeScale = 1;
    }
}
