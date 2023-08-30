using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnalogManager : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default;

    [Space(5)]
    [SerializeField] private Sprite _top;
    [SerializeField] private Sprite _bottom;

    [Space(5)]
    [SerializeField] private Sprite _left;
    [SerializeField] private Sprite _right;

    [Space(5)]
    [SerializeField] private Sprite _topLeft;
    [SerializeField] private Sprite _topRight;

    [Space(5)]
    [SerializeField] private Sprite _bottomLeft;
    [SerializeField] private Sprite _bottomRight;
    
    private Vector2 _moveInput;
    private void Update()
    {
        _moveInput = UnitController.main.moveInput;

        // Left and Right
        if (_moveInput.x < 0)
        {
            _img.sprite = _left;
        }
        else if (_moveInput.x > 0)
        {
            _img.sprite = _right;
        }
        

        // Top and Boton
        if (_moveInput.y > 0)
        {
            _img.sprite = _top;
        }
        else if (_moveInput.y < 0)
        {
            _img.sprite = _bottom;
        }

        // Top Left and Top Right
        if (_moveInput.x < 0 && _moveInput.y > 0)
        {
            _img.sprite = _topLeft;

        }
        else if (_moveInput.x > 0 && _moveInput.y > 0)
        {
            _img.sprite = _topRight;
        }

        // Top Left and Top Right
        if (_moveInput.x < 0 && _moveInput.y < 0)
        {
            _img.sprite = _bottomLeft;

        }
        else if (_moveInput.x > 0 && _moveInput.y < 0)
        {
            _img.sprite = _bottomRight;
        }

        if (UnitController.main.moveInput == new Vector2(0,0))
        {
            _img.sprite = _default;
        }
    }
}
