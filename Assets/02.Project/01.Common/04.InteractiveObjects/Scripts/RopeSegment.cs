using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public GameObject connectAbove;
    public GameObject connectBelow;

    public bool isPlayerAttached;

    private void Start()
    {
        connectAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveSegment = connectAbove.GetComponent<RopeSegment>();

        if(aboveSegment != null )
        {
            float spriteBotton = connectAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2 ( 0, spriteBotton*-1);
        }
        else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2 (0, 0);
        }

    }
}
