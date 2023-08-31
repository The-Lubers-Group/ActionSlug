using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDash : MonoBehaviour
{
    public static ShadowDash main;

    [SerializeField] private GameObject shadow;
    [SerializeField] private List<GameObject> list = new List<GameObject>();
    private float timer;
    [SerializeField] private float speed = 40;
    private void Awake()
    {
        main = this;
    }

    public GameObject GetShadow()
    {
        //Debug.Log("GetShadow()");
        for(int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                list[i].SetActive(true);
                list[i].transform.position = transform.position;
                list[i].transform.rotation = transform.rotation;
                return list[i];
            } 
        }
    
        GameObject obj = Instantiate(shadow, transform.position, transform.rotation) as GameObject;
        list.Add(obj);
        return obj;
    }

    public void Dash()
    {
        //Debug.Log("Dash()");
        timer += speed* Time.deltaTime;
        
        //Debug.Log("timer += speed* Time.deltaTime ->" + timer);
        if (timer > 0.1)
        {
            GetShadow();
            timer = 0;
        }
    }

}
