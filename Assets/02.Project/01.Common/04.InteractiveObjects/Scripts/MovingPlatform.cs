using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
    private float speed = 1.5f;
    int direction = 1;

    private void Update()
    {
        Vector2 target = CurrentMovementTarget();
        platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);
        
        float distance = (target - (Vector2)platform.position).magnitude;
        if (distance < 0.1f)
        {
            direction *= -1;
        }
    }

    Vector2 CurrentMovementTarget()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").transform.parent = null;
        }
    }
}
