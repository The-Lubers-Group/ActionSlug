using DG.Tweening;
using UnityEngine;

namespace LabLuby
{
    [RequireComponent(typeof(GameObject))]
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private GameObject _coin;
        [SerializeField] private SpriteRenderer _spriteIcon;

        [Space(10)]
        private float _durationTime = 1f;

        [Space(10)]
        [SerializeField] private AudioClip _soundFX;
        [SerializeField] private AudioSource _audioSource;

        private bool _aux = false;

        private void Update()
        {
            if (_aux && _coin != null)
            {
                _coin.transform.DOMove(MenuManager.main.UICoin.transform.position, _durationTime).OnComplete(() => RemoveCoin());
            }
        }

        private void RemoveCoin()
        {
            _spriteIcon.sortingOrder = 0;
            _coin.SetActive(false);
            //Destroy(_coin.gameObject);
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
    }

}
