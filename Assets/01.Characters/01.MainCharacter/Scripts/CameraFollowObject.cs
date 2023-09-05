using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LubyAdventure;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UnitController _mainPlayer;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRotationTime = 0.5f;

    private bool isFacingRight;
    //private Coroutine turnCoroutine;
   
    private void Update()
    {

        if (_mainPlayer == null)
        {
            _mainPlayer = UnitController.main;
            isFacingRight = _mainPlayer.IsFacingRight;
            transform.position = _mainPlayer.transform.position;
        }
    }

    public void CallTurn()
    {
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
