using DG.Tweening;
using DG.Tweening.Core.Easing;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform coin;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask interactLayer;

    private UnitController mainPlayer;

    private MenuManager menuManager;
    private SpriteRenderer spriteIcon;


    private void Start()
    {
        mainPlayer = FindAnyObjectByType<UnitController>();
        menuManager = FindAnyObjectByType<MenuManager>();
        spriteIcon = coin.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((interactLayer.value & 1 << collision.gameObject.layer) == interactLayer.value)
        {
            mainPlayer.Data.totalCoin += 1;
            spriteIcon.sortingOrder = 3;
            coin.DOMove(menuManager.UICoin.transform.position, 1);
            StartCoroutine(Wait(0.5f));
        }
    }
    IEnumerator Wait(float amt)
    {
        yield return new WaitForSeconds(amt);
        spriteIcon.sortingOrder = 0;
        Destroy(coin.gameObject);
    }
}
