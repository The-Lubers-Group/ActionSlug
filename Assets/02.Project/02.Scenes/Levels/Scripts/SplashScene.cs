using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            _scenesToLoad.Add(SceneManager.LoadSceneAsync(1));
            //Debug.Log("A key or mouse click has been detected");
        }
    }
}
