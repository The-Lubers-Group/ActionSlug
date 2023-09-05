using LubyAdventure;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager main;
    private UnitController mainCharacter;

    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private Canvas _mobileCanvas;
    [SerializeField] private Canvas _masterCanvas;
    [SerializeField] private Canvas _transitionsCanvas;

    [Space(5)]
    [SerializeField] private GameObject _UIPauseMenu;
    [SerializeField] private GameObject _UIRespawnMenu;
    [SerializeField] private GameObject _UIGameOverMenu;
    [Space(5)]

    // UI Text 
    [SerializeField] private TMP_Text _totalLife;
    [SerializeField] private TMP_Text _totalCoin;

    public GameObject UICoin;


    [Space(5)]
    public List<LetterManager> lubyLetter = new List<LetterManager>();

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }

        _menuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _mobileCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _masterCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _transitionsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        
    }
    private void Start()
    {
        _UIPauseMenu.SetActive(false);
        _UIRespawnMenu.SetActive(false);
        mainCharacter = UnitController.main;
    }

    private void Update()
    {
        _totalLife.text = "x " + mainCharacter.Data.totalLife.ToString();
        _totalCoin.text = "x " + mainCharacter.Data.totalCoin.ToString();

        if(mainCharacter.Data.totalCoin == 100)
        {
            mainCharacter.Data.totalLife += 1;
            mainCharacter.Data.totalCoin = 0;
        }
    }

    public void OnClickPauseMenu()
    {
        Time.timeScale = 0;
        _UIPauseMenu.SetActive(true);
    }

    public void CancelPauseMenu()
    {
        _UIPauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnLevelsMenu()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
    }

    public void OpenGameOverMenu()
    {
        Time.timeScale = 0;
        _UIGameOverMenu.SetActive(true);
        //mainCharacter.Data.totalLife--;
    }
     
    public void RestartGame()
    {
        GameManager.main.RestartGame();
        mainCharacter.SetAlive();
        //mainCharacter.RestartGame();
        _UIGameOverMenu.SetActive(false);
        _UIRespawnMenu.SetActive(false);
        
        Time.timeScale = 1;
    }


    public void OpenRestartMenu()
    {
        Time.timeScale = 0;
        _UIRespawnMenu.SetActive(true);
    }
}
