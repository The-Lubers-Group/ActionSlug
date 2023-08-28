using LubyAdventure;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private UnitController mainCharacter;

    [SerializeField] private GameObject UIPauseMenu;
    [SerializeField] private GameObject UIGameOverMenu;

    // UI Text 
    [SerializeField] private TMP_Text totalLife;
    [SerializeField] private TMP_Text totalCoin;

    public GameObject UICoin;


    [Space(5)]
    public List<LetterManager> lubyLetter = new List<LetterManager>();

    private void Start()
    {
        UIPauseMenu.SetActive(false);
        UIGameOverMenu.SetActive(false);
        mainCharacter = FindAnyObjectByType<UnitController>();
    }

    private void Update()
    {
        totalLife.text = "x " + mainCharacter.Data.totalLife.ToString();
        totalCoin.text = "x " + mainCharacter.Data.totalCoin.ToString();

        if(mainCharacter.Data.totalCoin == 100)
        {
            mainCharacter.Data.totalLife += 1;
            mainCharacter.Data.totalCoin = 0;
        }
    }

    public void OnClickPauseMenu()
    {
        Time.timeScale = 0;
        UIPauseMenu.SetActive(true);
    }

    public void CancelPauseMenu()
    {
        UIPauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnLevelsMenu()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
    }

    public void OnGameOverMenu()
    {
        Time.timeScale = 0;
        UIGameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        mainCharacter.Restart();
        UIGameOverMenu.SetActive(false);
        mainCharacter.SetAlive();
        Time.timeScale = 1;
    }
}
