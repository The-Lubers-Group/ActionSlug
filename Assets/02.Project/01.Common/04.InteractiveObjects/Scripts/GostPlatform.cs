using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostPlatform : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private float waitForSeconds = 1f;

    private bool visible = true;


    void Awake()
    {
        StartCoroutine("SetGuard");
    }



    IEnumerator SetGuard()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitForSeconds);
            Debug.Log("Sending");
            
            /*
            if (visible)
            {
                platform.SetActive(false);
            }else 
            {
                platform.SetActive(true);
            }
            */
        }
    }
}