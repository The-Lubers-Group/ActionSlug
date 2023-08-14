using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Level_", menuName = "Level/Unit/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public string LevelName;
    public int LevelNumber;

    public bool isCompleted;
    public int Score;
    public int nStar;

}