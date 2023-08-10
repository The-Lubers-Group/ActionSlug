using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform visualTransform;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask InteractLayer;

    [Header("Animator")]
    [SerializeField] private Animator anim;


    private Transform player;

    void Update()
    {
        if (Physics2D.OverlapBox(visualTransform.position, visualTransform.transform.localScale / 2, 0, InteractLayer))
        {
            //Debug.Log("Jogador");
            this.player = GameObject.FindWithTag("Player").transform;
            anim.SetTrigger("Trigger");
        }
    }

}
