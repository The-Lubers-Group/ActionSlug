using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelData Data;
    public static LevelData levelData;

    public static GameManager main;
    private void Awake()
    {
        if(main == null)
        {
            main = this;
        }
        levelData = Data;
    }
     
}


