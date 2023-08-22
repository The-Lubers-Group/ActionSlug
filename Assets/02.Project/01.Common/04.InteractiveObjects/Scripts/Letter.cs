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

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(letter.position, 0, InteractLayer))
        {
            UpdateLetterStatus();
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
}
