using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string IS_ATTACK = "isShoot";
    private const string IS_JUMP = "isJump";

    //[SerializeField] private PlayerManager mov;
    [SerializeField] private UnitController mov;
    [SerializeField] private Animator anim;
    //[SerializeField] private SpriteRenderer spriteRend;

    //private GameManager gameManager;

    [Header("Movement Tilt")]
    [SerializeField] private float maxTilt;
    [SerializeField][Range(0, 1)] private float tiltSpeed;

    [Header("Particle FX")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;
    private ParticleSystem jumpParticle;
    private ParticleSystem landParticle;

    public bool startedWalking { private get; set; }
    public bool startedAttacking { private get; set; }


    public bool startedJumping { private get; set; }
    public bool justLanded { private get; set; }

    public float currentVelY;
    //private PlayerManager playerManager;

    /*
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    */
   

    private void Start()
    {
        //mov = GetComponent<PlayerManager>();
        //spriteRend = GetComponentInChildren<SpriteRenderer>();
        //anim = spriteRend.GetComponent<Animator>();

        //gameManager = FindObjectOfType<GameManager>();

        //jumpParticle = jumpFX.GetComponent<ParticleSystem>();
        //landParticle = landFX.GetComponent<ParticleSystem>();
    }
  

    private void LateUpdate()
    {
        float tiltProgress;

        int mult = -1;

        if (mov.IsSliding)
        {
            tiltProgress = 0.25f;
        }
        else
        {
            tiltProgress = Mathf.InverseLerp(-mov.Data.runMaxSpeed, mov.Data.runMaxSpeed, mov.RB.velocity.x);
            mult = (mov.IsFacingRight) ? 1 : -1;
        }
        
        

        float newRot = ((tiltProgress * maxTilt * 2) - maxTilt);
        //float rot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, tiltSpeed);
        //spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);



       
        CheckAnimationState();

        ParticleSystem.MainModule jumpPSettings = jumpParticle.main;
        //jumpPSettings.startColor = new ParticleSystem.MinMaxGradient(demoManager.SceneData.foregroundColor);
        ParticleSystem.MainModule landPSettings = landParticle.main;
        //landPSettings.startColor = new ParticleSystem.MinMaxGradient(demoManager.SceneData.foregroundColor);
   
    }

    private void CheckAnimationState()
    {
        anim.SetBool(IS_WALKING, mov.IsWalking());
        
        
        Debug.Log(mov.IsWalking());
        //anim.SetBool(IS_ATTACK, mov.IsShoot());

        /*
        if(startedJumping)
        {
            anim.SetTrigger(IS_WALKING);
            startedWalking = false;
            return;
        }

        if (startedAttacking)
        {
            anim.SetTrigger(IS_ATTACK);
            startedAttacking = false;
            return;
        }
        */



        if (startedJumping)
        {
            anim.SetTrigger(IS_JUMP);
            GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            startedJumping = false;
            return;
        }

        if (justLanded)
        {
            anim.SetTrigger("Land");
            GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            justLanded = false;
            return;
        }

        anim.SetFloat("Vel Y", mov.RB.velocity.y);
    }
}
