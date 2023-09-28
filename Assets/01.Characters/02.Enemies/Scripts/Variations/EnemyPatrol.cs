using UnityEngine;

namespace LabLuby.Enemy.Variations
{
    [RequireComponent(typeof(Animator))]
    public class EnemyPatrol : BaseEnemy
    {

        private Rigidbody2D _rb;
        private Transform _currentPoint;

        [SerializeField] GameObject pointA;
        [SerializeField] GameObject pointB;

        void Start()
        {
            currentHealth = maxHealth;

            _rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            _currentPoint = pointB.transform;
            anim.SetBool("IsRunning", true);
        }

        void Update()
        {
            Vector2 point = _currentPoint.position - transform.position;
            if (_currentPoint == pointB.transform)
            {
                _rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                _rb.velocity = new Vector2(-speed, 0);
            }

            if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f && _currentPoint == pointB.transform)
            {
                Flip();
                _currentPoint = pointA.transform;
            }

            if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f && _currentPoint == pointA.transform)
            {
                Flip();
                _currentPoint = pointB.transform;
            }
        }

        private void Flip()
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
