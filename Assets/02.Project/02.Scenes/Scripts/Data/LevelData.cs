using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


[CreateAssetMenu(fileName = "Data_Level_", menuName = "Level/Unit/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public string LevelName;

    public string LevelSceneName;
    public int LevelID;

    public bool isCompleted;
    public int Score;
    public int nStar;
    
    //public bool[] lubyIcon = new bool[4];
    public List<LubyLetter> lubyLetter = new List<LubyLetter>()
    {
        new LubyLetter(){ letter  = "L", letterId = 1, letterStatus = false},
        new LubyLetter(){ letter  = "U", letterId = 2, letterStatus = false},
        new LubyLetter(){ letter  = "B", letterId = 3, letterStatus = false},
        new LubyLetter(){ letter  = "Y", letterId = 4, letterStatus = false}
    };
}

public class LubyLetter
{
    public string letter { get; set; }
    public int letterId { get; set; }
    public bool letterStatus { get; set; }
}
