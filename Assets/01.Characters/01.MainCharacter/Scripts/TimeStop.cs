using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{

    private float Speed;
    private bool RestoreTime;

    [Header("Animation Settings")]
    [SerializeField] private UnitCharacterAnimationBehaviour characterAnimationBehaviour;

    private void Start()
    {
        RestoreTime = false;
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
            }
        }
    }

    public void StopTime(float ChangeTime, int RestoreSpeed, float Delay)
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

        characterAnimationBehaviour.CharacterWasHit();
        Debug.Log("Dano");

        Time.timeScale = ChangeTime;
    }

    IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSeconds(amt);
        RestoreTime = true;
    }
}
