using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform letter;

    [Header("ID")]
    [SerializeField] private int letterId;
    
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask InteractLayer;

    private GameManager gameManager;
    private MenuManager menuManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(letter.position, 0, InteractLayer))
        {
            UpdateLetterStatus();
            OnLetterAnim();
            Destroy(letter.gameObject);
        }
    }
    private void UpdateLetterStatus()
    {
        foreach (LubyLetter lubyLetter in gameManager.Data.lubyLetter)
        {
            if (lubyLetter.letterId == letterId)
            {
                lubyLetter.letterStatus = true;
            }
        }
    }


    private void OnLetterAnim()
    {
        for (int i = 0; i < menuManager.lubyLetter.Count; i++)
        {
            if((i + 1) == letterId)
            {
                //letter.position = new Vector3(1.0f, 1.0f, 1.0f);
                letter.position = menuManager.lubyLetter[i].transform.position ;
            }
        }
    }
}
