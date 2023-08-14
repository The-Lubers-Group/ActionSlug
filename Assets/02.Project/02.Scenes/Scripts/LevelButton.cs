using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public LevelData Data;
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();


    public void Onclick()
    {
        Debug.Log(Data.LevelName);
        scenesToLoad.Add(SceneManager.LoadSceneAsync(Data.LevelSceneName));

    }

}
