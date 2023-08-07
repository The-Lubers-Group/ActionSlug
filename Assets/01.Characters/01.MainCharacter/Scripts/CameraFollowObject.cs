using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTranform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRotationTime = 0.5f;

    //private PlayerManager player;
    private UnitController player;
    private bool isFacingRight;
    private Coroutine turnCoroutine;
    
    private void Awake()
    {
        player = playerTranform.gameObject.GetComponent<UnitController>();
        isFacingRight = player.IsFacingRight;
    }

    private void Update()
    {
        // Make the cameraFollowObject follow the player's position 
        transform.position = playerTranform.position;
    }

    public void CallTurn()
    {
        // turnCoroutine = StartCoroutine(FlipYLerp());
        //transform.DORotate(gameObject, DetermineEndRotation(), flipYRotationTime);
        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime).setEaseInOutSine();

    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float eLapsedTime = 0f;
        while(eLapsedTime < flipYRotationTime) 
        {
            eLapsedTime += Time.deltaTime;

            //lerp the y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (eLapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }  


    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if(isFacingRight )
        {
            return 180f;
        }
        else 
        { 
            return 0;
        }
    }

}
