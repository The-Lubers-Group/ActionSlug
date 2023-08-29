using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosting : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Color color;

    private void Start()
    {
        OnMaterial();
    }


    private void OnMaterial()
    {
        foreach (Transform child in transform.GetChild(0).GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<SpriteRenderer>())
            {
                child.GetComponent<SpriteRenderer>().material = flashMaterial;
                child.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }

}
