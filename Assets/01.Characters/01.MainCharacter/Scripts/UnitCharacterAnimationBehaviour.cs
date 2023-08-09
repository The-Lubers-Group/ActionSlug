using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace LubyAdventure
{

    public class UnitCharacterAnimationBehaviour : MonoBehaviour
    {
        [Header("Animator")]
        public Animator characterAnimator;
        public UnitController unitController;
        private SpriteRenderer spriteRend;

        [Header("Movement Tilt")]
        [SerializeField] private float maxTilt;
        [SerializeField][Range(0, 1)] private float tiltSpeed;


        private string animGetHitParameter = "Get Hit";
        private int animGetHitID;

        private string animDieParameter = "Die";
        private int animDieID;

        private string animWalkingParameter = "isWalking";
        private int animWalkingID;

        private string animJumpParameter = "Jump";
        private int animJumpID;

        private string animLandParameter = "Land";
        private int animLandID;


        public bool startedJumping { private get; set; }
        public bool justLanded { private get; set; }
        
        void Awake()
        {
            SetupAnimationIDs();
        }


        private void Start()
        {
            spriteRend = GetComponentInChildren<SpriteRenderer>();
        }


        private void LateUpdate()
        {

            float tiltProgress;

            int mult = -1;

            if (unitController.IsSliding)
            {
                tiltProgress = 0.25f;
            }
            else
            {
                tiltProgress = Mathf.InverseLerp(-unitController.Data.runMaxSpeed, unitController.Data.runMaxSpeed, unitController.RB.velocity.x);
                mult = (unitController.IsFacingRight) ? 1 : -1;
            }

            float newRot = ((tiltProgress * maxTilt * 2) - maxTilt);
            float rot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, tiltSpeed);
            spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);
            
            CheckAnimationState();
        }

        void SetupAnimationIDs()
        {
            animGetHitID = Animator.StringToHash(animGetHitParameter);
            animDieID = Animator.StringToHash(animDieParameter);
            animWalkingID = Animator.StringToHash(animWalkingParameter);
            animJumpID = Animator.StringToHash(animJumpParameter);
            animLandID = Animator.StringToHash(animLandParameter);
        }

        public void CharacterWasHit()
        {
            characterAnimator.SetTrigger(animGetHitID);
        }

        public void CharacterHasDied()
        {
            characterAnimator.SetTrigger(animDieID);
        }

        private void CheckAnimationState()
        {
            characterAnimator.SetBool(animWalkingID, unitController.IsWalking());
            if (startedJumping)
            {
                characterAnimator.SetTrigger(animJumpID);
                startedJumping = false;
                return;
            }

            if (justLanded)
            {
                characterAnimator.SetTrigger(animLandID);
                justLanded = false;
                return;
            }

            characterAnimator.SetFloat("Vel Y", unitController.RB.velocity.y);
        }

    }
}
