using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    private bool _isSwimming;
    private UnitInfoData _data;
    private UnitCharacterAnimationBehaviour _characterAnim;
    private UnitController _mainController;

    [SerializeField] private Vector2 _defaultOffset;
    [SerializeField] private Vector2 _defaultSize;
    
    private void Start()
    {
        _mainController = GetComponent<UnitController>();

        _data = _mainController.Data;
        _isSwimming = _mainController.IsSwimming;
        _characterAnim = _mainController.characterAnimationBehaviour;
    }

    public void CanSwim(bool isSwimming, Vector2 moveInput, CapsuleCollider2D capsuleCollider2D)
    {
        if (capsuleCollider2D.direction == CapsuleDirection2D.Vertical)
        {
            _defaultSize = capsuleCollider2D.size;
            _defaultOffset = capsuleCollider2D.offset;
        }

        capsuleCollider2D.offset = new Vector2(3f, .26f);
        capsuleCollider2D.size = new Vector2(8.5f, 3.2f);
        
        if (isSwimming)
        {

            capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;

            _characterAnim.SwimmingAnim(true);
            _mainController.RB.drag = _data.linerDragSwimming;

            if (moveInput.x == 0 && moveInput.y == 0)
            {
                //Physics2D.gravity = new Vector2(0, 0);
                Physics2D.gravity = new Vector2(0, -_data.gravitySwimming/2);

            }

            if (moveInput.x > 0) //Right
            {
                Physics2D.gravity = new Vector2(_data.gravitySwimming, 0);
            }
            else if (moveInput.x < 0) //Left
            {
                Physics2D.gravity = new Vector2(-_data.gravitySwimming, 0);
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
            capsuleCollider2D.direction = CapsuleDirection2D.Vertical;
            capsuleCollider2D.size = _defaultSize;
            capsuleCollider2D.offset = _defaultOffset;

            _characterAnim.SwimmingAnim(false);
            _mainController.RB.drag = _data.linerDrag;
            Physics2D.gravity = new Vector2(0, -9.81f);
        }
    }
}

                 //_moveInput = _mainController.moveInput;
                //Physics2D.gravity = new Vector2(-_data.gravitySwimming, 0);
                //Physics2D.gravity = new Vector2(_data.gravitySwimming, 0);