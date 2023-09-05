using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private List<LevelData> _levelList = new List<LevelData>();
    [SerializeField] private List<Button> _buttonlevelList = new List<Button>();
    
    private LineRenderer _lineRenderer;
    private Vector3 pos;
    public void OnMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void Start()
    {
        for (int i = 0; i < _levelList.Count; i++)
        {
            if (!_levelList[i].isBlocked)
            {
                print(_buttonlevelList[i].name);
            }
        }
    }


    private void Update()
    {
    }


    private void Options()
    {
        _lineRenderer.positionCount = _buttonlevelList.Count;

        for (int i = 0; i < _buttonlevelList.Count; i++)
        {
            print(_levelList[i].isBlocked);
            if (!_levelList[i].isBlocked)
            {
                pos = _buttonlevelList[i].transform.position;
                _lineRenderer.SetPosition(i, new Vector3(pos.x, pos.y, 0));
            }
        }
    }
}


/**

  for (int i = 0; i < _buttonlevelList.Count; i++) {
            //if ((i % 2 == 0) && i == 0)
            ///{
                ///Debug.Log("_buttonlevelList[i]: " + _buttonlevelList[i].name);
                pos = _buttonlevelList[i].transform.position;
                _lineRenderer.SetPosition(i, new Vector3(pos.x, pos.y, 0));
            //}
        }

 */