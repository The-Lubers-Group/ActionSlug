using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterManager : MonoBehaviour
{
    [SerializeField] private Image iconLetter;
    
    [SerializeField] private Sprite enableLetter;
    [SerializeField] private Sprite disableLetter;
    [SerializeField] private int ID;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        GetLetterStatus();
    }


    private void GetLetterStatus()
    {
        foreach (LubyLetter lubyLetter in gameManager.Data.lubyLetter)
        {
            if(lubyLetter.letterId == ID)
            {
                if (lubyLetter.letterStatus)
                {
                    //iconLetter.sprite = enableLetter;
                    StartCoroutine(Wait(0.5f));

                }
                if (!lubyLetter.letterStatus)
                {
                    iconLetter.sprite = disableLetter;
                }
            }
        }
    }

    IEnumerator Wait(float amt)
    {
        yield return new WaitForSeconds(amt);
        iconLetter.sprite = enableLetter;
    }
}
