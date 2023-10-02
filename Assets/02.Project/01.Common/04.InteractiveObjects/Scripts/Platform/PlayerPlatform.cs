using DG.Tweening;
using UnityEngine;

namespace LabLuby.Platform
{
    public class PlayerMovingPlatfor : MonoBehaviour
    {
        [SerializeField] private float _time = 2f;

        [Space(10)]
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

        private PlatformEffector2D _platformEffector2D;
        private GameInput gameInput;
        private Transform _platform;
        private Animator _animator;
        private Vector3 _velocity;
        private int _direction;
        private Vector3 _pos;

        private void Awake()
        {
            _platformEffector2D = GetComponent<PlatformEffector2D>();
            _platform = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
            _pos = transform.position;
            _direction = 1;
        }
        private void Update()
        {
            Vector2 target = CurrentMovementTarget();
            _platform.DOMove(target, _time).SetEase(Ease.Linear);

            _velocity = (transform.position - _pos) / Time.deltaTime;
            _pos = transform.position;

            if (_velocity.magnitude > 1)
            {
                _animator.SetBool("isMoving", true);
            }
            else
            {
                _animator.SetBool("isMoving", false);
            }
        }

        Vector2 CurrentMovementTarget()
        {
            if (_direction == 1)
            {
                return _startPoint.position;
            }
            else
            {
                return _endPoint.position;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                _direction = -1;
                if (transform.position.y < collision.transform.position.y)
                {
                    collision.transform.SetParent(transform);
                    gameInput = GameObject.FindAnyObjectByType<GameInput>();
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (gameInput == null)
            {
                return;
            }

            if (gameInput.GetMoveLeftRight())
            {
                collision.transform.SetParent(null);
            }
            else
            {
                collision.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _direction = 1;
                _platformEffector2D.rotationalOffset = 0;
                collision.transform.SetParent(null);
                gameInput = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawLine(_startPoint.position, new Vector3(_startPoint.position.x, _endPoint.position.x));
        }
    }
}
