using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float speedParallax;
    private float length;
    private float startPos;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (mainCamera.transform.position.x * (1 - speedParallax));
        float dist = (mainCamera.transform.position.x * speedParallax);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(temp > startPos + length /2)
        {
            startPos += length;
        }
        else if ( temp < startPos - length /2)
        {
            startPos -= length;
        }
    }

}
