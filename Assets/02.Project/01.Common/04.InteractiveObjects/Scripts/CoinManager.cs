using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform Coin;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask InteractLayer;
    private void Update()
    {
        if (Physics2D.OverlapCircle(Coin.position,  0, InteractLayer))
        {
            Destroy(Coin.gameObject);
        }
    }
}
