using DG.Tweening;
using LubyAdventure;
using System.Collections;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Main Object")]
    [SerializeField] private Transform _coin;

    //[Header("Layers & Tags")]
    //[SerializeField] private LayerMask interactLayer;

    [SerializeField] private AudioClip _soundFX;
    [SerializeField] private AudioSource _audioSource;

    private bool _aux = false;

    private SpriteRenderer _spriteIcon;
    private void Start()
    {
        _coin = transform.GetComponent<Transform>();
        _spriteIcon = _coin.GetChild(0).GetComponentInChildren<SpriteRenderer>();

    }

    private void Update()
    {
        if (_aux)
        {
            _coin.DOMove(MenuManager.main.UICoin.transform.position, 1);
            StartCoroutine(Wait(0.5f));

        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _audioSource.PlayOneShot(_soundFX);
            MenuManager.main.AddCoin(1);
            _spriteIcon.sortingOrder = 3;
            _aux = true;
        }
    }

    IEnumerator Wait(float amt)
    {
        yield return new WaitForSeconds(amt);
        _spriteIcon.sortingOrder = 0;
        Destroy(_coin.gameObject);
    }
}
