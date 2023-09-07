using DG.Tweening;
using LubyAdventure;
using System.Collections;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform coin;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask interactLayer;

    private SpriteRenderer spriteIcon;
    private void Start()
    {
        spriteIcon = coin.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((interactLayer.value & 1 << collision.gameObject.layer) == interactLayer.value)
        {
            //UnitController.main.Data.totalCoin += 1;
            MenuManager.main.AddCoin(1);
            spriteIcon.sortingOrder = 3;
            coin.DOMove(MenuManager.main.UICoin.transform.position, 1);
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
