using LabLuby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<UnitController>().GetHit();
            //collision.gameObject.GetComponent<GetHit>().StopTime(0.5F, 10, 0.1F);
            collision.gameObject.GetComponent<UnitController>().PlayerHit(1);
        }
    }
}
 