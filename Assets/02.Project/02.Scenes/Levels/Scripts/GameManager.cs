using Cinemachine;
using Collections.Shaders.CircleTransition;
using DG.Tweening;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public static LevelData levelData;
    
    [SerializeField] private LevelData _levelData;
    
    [Space(5)]
    [SerializeField] private Transform _UIMenu;
    [SerializeField] private UnitController _mainPlayer;
    
    [Space(10)]
    public GameObject startPoint;
    
    private void Awake()
    {
        if(main == null)
        {
            main = this;
        }
        levelData = _levelData;
    }
    private void Start()
    {
        Instantiate(_UIMenu, new Vector3(0,0,0), Quaternion.identity);

        _mainPlayer = Instantiate(_mainPlayer, startPoint.transform.position, Quaternion.identity);
        _mainPlayer.cameraFollowObject = FindAnyObjectByType<CameraFollowObject>();
        FindAnyObjectByType<CinemachineVirtualCamera>().m_Follow = _mainPlayer.transform;
}

    private void Update()
    {
        if(_mainPlayer.Data.totalLife <= 0)
        {
            //CircleTransition.main.CloseBlackScreen();
            
            _mainPlayer.Data.totalLife = 0;
            MenuManager.main.OpenGameOverMenu();
        }
        else if (UnitController.playerLife <= 0)
        {
            MenuManager.main.OpenRestartMenu();
        }
    }
    public void RestartGame()
    {
        _mainPlayer.transform.position = startPoint.transform.position;
    }
}


