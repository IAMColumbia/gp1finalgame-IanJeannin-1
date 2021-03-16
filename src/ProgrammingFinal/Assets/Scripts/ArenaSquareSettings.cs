using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArenaSquareSettings
{
   //==========================================
    // ArenaSquare Variables
    //==========================================
    [Tooltip("How long the player can stay on a square before it breaks.")]
    [SerializeField]
    public static float maxDurability = 5;
    [Tooltip("How long the player must stay off the square for it to start regenerating")]
    [SerializeField]
    public static float regeneratePeriod = 0.1f;
    [Tooltip("How much the object regenerates per second.")]
    [SerializeField]
    public static float regenerateRate = 0.25f;
    [Tooltip("After durability goes to 0, how long the player has before it breaks completely.")]
    [SerializeField]
    public static float breakPeriod = 1;

    public static Color32 startingColor;
    public static Color32 middleColor = new Color32(255, 255, 0, 255);
    public static Color32 endColor = new Color32(255, 0, 0, 255);


    public static void ChangeDifficulty()
    {
        switch (Menu.difficulty)
        {
            case Menu.Difficulty.easy:
                SetEasy();
                break;
            case Menu.Difficulty.medium:
                SetMedium();
                break;
            case Menu.Difficulty.hard:
                SetHard();
                break;
        }
    }

    public static void SetEasy()
    {
        maxDurability = 5;
        regeneratePeriod = 1;
        regenerateRate = 0.25f;
        breakPeriod = 1.5f;
    }

    public static void SetMedium()
    {
        maxDurability = 3;
        regeneratePeriod = 1;
        regenerateRate = 0.2f;
        breakPeriod = 1;
    }

    public static void SetHard()
    {
        maxDurability = 2;
        regeneratePeriod = 1.5f;
        regenerateRate = 0.2f;
        breakPeriod = 0.5f;
    }
}
