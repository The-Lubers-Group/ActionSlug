using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string IS_SHOOT = "isShoot";
    private const string IS_JUMP = "isJump";

    private Animator animator;

    public bool startedJumping { private get; set; }


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

    private void LateUpdate()
    {
        CheckAnimationState();

    }

    private void CheckAnimationState()
    {
        if (startedJumping)
        {
            animator.SetTrigger(IS_JUMP);
            //animator.SetBool(IS_JUMP, playerManager.);
            startedJumping = false;
            return;
        }
    }
}
