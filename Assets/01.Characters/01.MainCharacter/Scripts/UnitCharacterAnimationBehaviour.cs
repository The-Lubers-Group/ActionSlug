using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace LabLuby
{

    public class UnitCharacterAnimationBehaviour : MonoBehaviour
    {
        [Header("Animator")]
        public Animator characterAnimator;
        [HideInInspector] public UnitController unitController;
        private SpriteRenderer spriteRend;

        [Header("Movement Tilt")]
        [SerializeField] private float maxTilt;
        [SerializeField][Range(0, 1)] private float tiltSpeed;


        [Header("Particle FX")]
        [SerializeField] private GameObject jumpFX;
        [SerializeField] private GameObject landFX;


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

        private string animIsDashingParameter = "IsDashing";
        private int animIsDashingID;

        private string animVelXParameter = "Vel X";
        private int animVelXID;

        private string animVelYParameter = "Vel Y";
        private int animVelYID;

        private string animSwimXTimeParameter = "Press Swim X";
        private int animSwimXTimeID;

        public bool startedJumping { private get; set; }
        public bool justLanded { private get; set; }
        
        void Awake()
        {
            SetupAnimationIDs();
        }


        private void Start()
        {
            spriteRend = GetComponentInChildren<SpriteRenderer>();
            unitController = UnitController.main;
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
            animIsDashingID = Animator.StringToHash(animIsDashingParameter);
            
            animVelXID = Animator.StringToHash(animVelXParameter);
            animVelYID = Animator.StringToHash(animVelYParameter);

            animSwimXTimeID = Animator.StringToHash(animSwimXTimeParameter);
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
                GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-90, 0, 0));
                Destroy(obj,1);
                startedJumping = false;
                return;
            }

            if (justLanded)
            {
                characterAnimator.SetTrigger(animLandID);
                GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
                Destroy(obj, 1);
                justLanded = false;
                return;
            }
            
            characterAnimator.SetFloat(animVelYID, unitController.RB.velocity.y);
            characterAnimator.SetFloat(animVelXID, unitController.RB.velocity.x);
            characterAnimator.SetFloat(animSwimXTimeID, Swim.LastPressedSwimXTime);
        }

        public void SwimmingAnim(bool status)
        {
            characterAnimator.SetBool("Swimming", status);
        }
        
        public void SetWallSliderAnim(bool status)
        {
            characterAnimator.SetBool("IsSlider", status);
        }

        public void OnLedgeClimbAnim(bool status)
        {
            characterAnimator.SetBool("LedgeClimb", status);

        }
        public void OnIsDashingAnim(bool status)
        {
            characterAnimator.SetBool(animIsDashingID, status);

        }
    }
}
