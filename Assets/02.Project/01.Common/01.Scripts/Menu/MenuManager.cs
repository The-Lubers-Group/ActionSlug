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
    [SerializeField] private TMP_Text _totalLife;
    [SerializeField] private TMP_Text _totalCoin;

    [Space(5)]
    public GameObject UICoin;
    
    [Space(5)]
    public List<LetterManager> lubyLetter = new List<LetterManager>();

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }
    private void Start()
    {
        _UIPauseMenu.SetActive(false);
        _UIRespawnMenu.SetActive(false);
        mainCharacter = UnitController.main;

        _menuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _mobileCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _masterCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _transitionsCanvas.renderMode = RenderMode.ScreenSpaceCamera;

        _menuCanvas.worldCamera = Camera.main;
        _mobileCanvas.worldCamera = Camera.main;
        _masterCanvas.worldCamera = Camera.main;
        _transitionsCanvas.worldCamera = Camera.main;

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
    }
     
    public void RestartGame()
    {
        GameManager.main.RestartGame();
        mainCharacter.SetAlive();
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
