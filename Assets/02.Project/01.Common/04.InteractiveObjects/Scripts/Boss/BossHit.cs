using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BossHit : MonoBehaviour
{

    private void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindAnyObjectByType<GruzMother>().GetDamage();
        }
    }


    IEnumerator Wait(float _time)
    {
        yield return new WaitForSeconds(_time);
        //Components();

    }
}
