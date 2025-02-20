using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _press, _hover;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;


    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _press;
        _source.PlayOneShot(_compressClip);
        transform.DOShakePosition(3.0f, strength: new Vector3(0, 4, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;
        _source.PlayOneShot(_uncompressClip);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _img.sprite = _hover;
        //transform.DOShakePosition(3.0f, strength: new Vector3(0, 4, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true).OnComplete(() => _img.sprite = _default);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _img.sprite = _default;
        
    }
    
}
