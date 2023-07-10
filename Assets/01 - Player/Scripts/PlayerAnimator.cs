using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string IS_SHOOT = "isShoot";

    private Animator animator;

    [SerializeField] private PlayerManager playerManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, playerManager.IsWalking());
        animator.SetBool(IS_SHOOT, playerManager.IsShoot());
    }
}
