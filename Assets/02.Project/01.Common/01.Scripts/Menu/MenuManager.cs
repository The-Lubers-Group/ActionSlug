using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject UIPauseMenu;

    private void Start()
    {
        UIPauseMenu.SetActive(false);
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
