using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetTime;
    
    private float _lifetime;
    private static string TAG_IGNORE = "Ignore";

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
            gameObject.SetActive(false);
        }
    }
}
