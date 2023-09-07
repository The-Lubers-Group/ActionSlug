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

    private SpriteRenderer spriteIcon;

    [SerializeField] private ParticleSystem _particle;

    private void Start()
    {
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
        foreach (LubyLetter lubyLetter in GameManager.levelData.lubyLetter)
        {
            if (lubyLetter.letterId == letterId)
            {
                lubyLetter.letterStatus = true;
                Destroy(_particle);
            }
        }
    }
    private void OnLetterAnim()
    {
        for (int i = 0; i < MenuManager.main.lubyLetter.Count; i++)
        {
            if((i + 1) == letterId)
            {
                spriteIcon.sortingOrder = 3;
                letter.DOMove(MenuManager.main.lubyLetter[i].transform.position, 1);
            }
        }
    }
    IEnumerator Wait(float amt)
    {
        yield return new WaitForSeconds(amt);
        spriteIcon.sortingOrder = 0;
        //Destroy(letter.gameObject);
        Destroy(letter);
    }
}
