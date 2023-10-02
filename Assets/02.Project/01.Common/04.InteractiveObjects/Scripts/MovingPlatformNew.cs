using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformNew : MonoBehaviour
{

    [SerializeField] private float speed = 5f;          // speed of the platform
    [SerializeField] private int startinPoint;          // starting index (position of the platform)
    [SerializeField] private Transform[] points;        // An array of transform points (positions where the platform needs to move)
    
    [SerializeField] private PlatformEffector2D platformEffector2D; 
    
    
    [SerializeField] private GameInput gameInput;        // GameInput Reference (Use the new input system)


    private int i; //index of the array;

    private void Start()
    {
        transform.position = points[startinPoint].position; // Setting the position of the platform to
                                                            // the position of one of the points using index "startingPoints"
    }

    private void Update()
    {
        // checking th distance of the platform and the point
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++; // increse the index
            if (i == points.Length) // Check if the platform was on the last point after the index increase
            {
                i = 0; // reset the index;
            }
        }
        // moving the platform to the point position with the index "i"
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if(transform.position.y < collision.transform.position.y)
            {
                collision.transform.SetParent(transform);
                gameInput = GameObject.FindAnyObjectByType<GameInput>();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(gameInput == null)
        {
            return;
        }

        if (gameInput.GetMoveLeftRight())
        {
            collision.transform.SetParent(null);
        }
        else
        {
            collision.transform.SetParent(transform);
        }
        /*
        if (gameInput.GetMoveUpDown())
        {
            collision.transform.SetParent(null);
            platformEffector2D.rotationalOffset = 180;
            gameInput = null;
        }
        */

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        platformEffector2D.rotationalOffset = 0;
        collision.transform.SetParent(null);
        gameInput = null;

    }
}
/*
 * 
 * private void OnCollisionStay2D(Collision2D collision)
    {
        if(gameInput == null)
        {
            return;
        }
        if (gameInput.GetMoveUpDown())
        {
            collision.transform.SetParent(null);
            platformEffector2D.rotationalOffset = 180;
            gameInput = null;
        } 
        if (gameInput.GetMoveLeftRight())
        {
            collision.transform.SetParent(null);
            gameInput = null;
        }
        else
        {
            collision.transform.SetParent(transform);
        }
    }
 **/