using Collections.Shaders.CircleTransition;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    public static GetHit main;

    private float Speed;
    private bool RestoreTime;

    [Header("Animation Settings")]
    [SerializeField] private UnitCharacterAnimationBehaviour characterAnimationBehaviour;

    //[Header("Visual Effect")]
    //[SerializeField] private GameObject impactEffect;

    [Header("Particle FX")]
    [SerializeField] private GameObject impactFX;

    private ParticleSystem _jumpParticle;

    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Material originalMaterial;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        RestoreTime = false;
        _jumpParticle = impactFX.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(RestoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * Speed;
            }
            else
            {
                Time.timeScale = 1f;
                RestoreTime = false;
                OnMaterial(false);
            }
        }
    }

    public void Hit(float ChangeTime, int RestoreSpeed, float Delay)
    {
        Speed = RestoreSpeed;

        if(Delay > 0)
        {
            StopCoroutine(StartTimeAgain(Delay));
            StartCoroutine(StartTimeAgain(Delay));
        }
        else
        {
            RestoreTime =  true;
        }


        GameObject obj = Instantiate(impactFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-40, 0, 0));
        Destroy(obj, 1);


        ParticleSystem.MainModule jumpPSettings = _jumpParticle.main;
        jumpPSettings.startColor = new ParticleSystem.MinMaxGradient(Color.red);
        //Instantiate(impactFX, impactFX.transform.position, Quaternion.identity);
        characterAnimationBehaviour.CharacterWasHit();
        
        transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        
        OnMaterial(true);
        Time.timeScale = ChangeTime;

    }
    private void OnMaterial(bool status)
    {
        foreach (Transform child in transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>())
        {
            //Debug.Log(child.name);
            if (child.GetComponent<SpriteRenderer>())
            {
                if(status)
                {
                    child.GetComponent<SpriteRenderer>().material = flashMaterial;
                }
                else
                {
                    child.GetComponent<SpriteRenderer>().material = originalMaterial;

                }
            }
        }
    }
    IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSeconds(amt);
        RestoreTime = true;
    }
}
