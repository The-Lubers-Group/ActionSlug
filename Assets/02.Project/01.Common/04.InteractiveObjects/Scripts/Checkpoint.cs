using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Object")]
    //[SerializeField] private Transform visualTransform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject textCheckpoint ;

    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private Vector3 Offset = new Vector3 (0, 10, 0);
    private GameObject text ;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask InteractLayer;

    [Header("Animator")]
    [SerializeField] private Animator anim;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((InteractLayer.value & 1 << collision.gameObject.layer) == InteractLayer.value)
        {
            text = Instantiate(textCheckpoint, transform.position, Quaternion.identity, transform);
            
            text.transform.transform.DOScale(1.1f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            //text.transform.DOMove(text.transform.position += Offset, 1);
            text.transform.localPosition += Offset;

            GetComponent<Collider2D>().enabled = false;

            startPoint.position = transform.position;
            anim.SetTrigger("Trigger");
            Destroy(text, destroyTime);
        }
    }


}
