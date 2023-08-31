using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private LevelData levelData;

    private void Start()
    {
       levelData = GameManager.levelData;  
    }


    public void ClickNext()
    {
        SceneManager.LoadSceneAsync(levelData.LevelID + 2);
    }

    public void ClickRetry()
    {
        SceneManager.LoadSceneAsync(levelData.LevelID + 1);
    }

    public void ClickMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
