using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform visualTransform;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask InteractLayer;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private void Update()
    {
        if (Physics2D.OverlapCircle(visualTransform.position,  0, InteractLayer))
        {
            //Application.LoadLevel("GameOver");
            scenesToLoad.Add(SceneManager.LoadSceneAsync("GameOver"));
        }
    }
}
