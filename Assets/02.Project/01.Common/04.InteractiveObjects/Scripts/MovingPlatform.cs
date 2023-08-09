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

    private void OnDrawGizmos()
    {
        if(platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.transform.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.transform.position);
        }
    }

}
