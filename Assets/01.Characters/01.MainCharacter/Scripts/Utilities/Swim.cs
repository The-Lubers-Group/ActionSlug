using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    private UnitInfoData _data;
    private UnitCharacterAnimationBehaviour _characterAnim;
    private UnitController _mainController;
    
    public static float LastPressedSwimXTime;

    private void Start()
    {
        _mainController = GetComponent<UnitController>();

        _data = _mainController.Data;
        _characterAnim = _mainController.characterAnimationBehaviour;
    }
    public void CanSwim(bool isSwimming, Vector2 moveInput)
    {
        

        if (isSwimming)
        {
            _characterAnim.SwimmingAnim(true);
            _mainController.RB.drag = _data.linerDragSwimming;

            if (moveInput.x == 0 && moveInput.y == 0)
            {
                Physics2D.gravity = new Vector2(0, -_data.gravitySwimming / 2);
                LastPressedSwimXTime = 0;
            }
            if (moveInput.x > 0)
            {
                Physics2D.gravity = new Vector2(_data.gravitySwimming, 0);
                LastPressedSwimXTime += Time.deltaTime;

            }
            else if (moveInput.x < 0)
            {
                Physics2D.gravity = new Vector2(-_data.gravitySwimming, 0);
                LastPressedSwimXTime += Time.deltaTime;
            }


            if (moveInput.y > 0)
            {
                Physics2D.gravity = new Vector2(0, _data.gravitySwimming);
            }
            else if (moveInput.y < 0)
            {
                Physics2D.gravity = new Vector2(0, -_data.gravitySwimming);
            }


        }
        else if (!isSwimming)
        {
            _characterAnim.SwimmingAnim(false);
            _mainController.RB.drag = _data.linerDrag;
            Physics2D.gravity = new Vector2(0, -9.81f);
        }
    }
}
