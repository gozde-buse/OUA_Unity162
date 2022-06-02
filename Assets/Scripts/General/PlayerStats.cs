using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static int level { get; private set; }
    public static int[] levelStars { get; private set; } = new int[1];

    public static void SetLevel(int level)
    {
        PlayerStats.level = level;
    }

    public static void SetLevelStar(int index, int starCount)
    {
        if(levelStars[index] < starCount)
            levelStars[index] = starCount;
    }
}