using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private SpriteRenderer spriteIcon;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        menuManager = FindAnyObjectByType<MenuManager>();
        spriteIcon = letter.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(letter.position, 0, InteractLayer))
        {
            UpdateLetterStatus();
            OnLetterAnim();
            StartCoroutine(Wait(0.5f));

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
                spriteIcon.sortingOrder = 3;
                letter.DOMove(menuManager.lubyLetter[i].transform.position, 1);
            }
        }
    }
    IEnumerator Wait(float amt)
    {
        yield return new WaitForSeconds(amt);
        spriteIcon.sortingOrder = 0;
        Destroy(letter.gameObject);
    }
}
