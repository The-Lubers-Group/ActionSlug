using LubyAdventure;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private UnitController mainCharacter;

    [SerializeField] private GameObject UIPauseMenu;

    // UI Text 
    [SerializeField] private TMP_Text totalLife;
    [SerializeField] private TMP_Text totalCoin;

    private void Start()
    {
        UIPauseMenu.SetActive(false);
        mainCharacter = FindAnyObjectByType<UnitController>();
    }

    private void Update()
    {
        totalLife.text = "x " + mainCharacter.Data.totalLife.ToString();
        totalCoin.text = "x " + mainCharacter.Data.totalCoin.ToString();

        if(mainCharacter.Data.totalCoin == 100)
        {
            mainCharacter.Data.totalLife += 1;
            mainCharacter.Data.totalCoin = 0;
        }
    }

    public void OnClickPauseMenu()
    {
        Time.timeScale = 0;
        UIPauseMenu.SetActive(true);
    }

    public void CancelPauseMenu()
    {
        UIPauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
