using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private List<LevelData> levelList = new List<LevelData>();
    [SerializeField] private List<Button> ButtonlevelList = new List<Button>();

    public void OnMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
