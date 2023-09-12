using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetTime;
    
    [Space(5)]
    [SerializeField] private AudioClip _soundFX;

    private float _lifetime;
    private Animator _animator;
    private AudioSource _audioSource;

    private static string TAG_IGNORE = "Ignore";
    private static string TAG_TRIGGER = "HitTrigger";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetDirection()
    {
        _lifetime = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        float _movementSpeed = _speed * Time.deltaTime;
        transform.Translate(_movementSpeed, 0,0);

        _lifetime += Time.deltaTime;
        if(_lifetime > _resetTime) 
        {
            gameObject.SetActive(false);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        if (collision.gameObject.tag != TAG_IGNORE)
        {
            _animator.SetTrigger(TAG_TRIGGER);
            _audioSource.PlayOneShot(_soundFX);
        }
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
