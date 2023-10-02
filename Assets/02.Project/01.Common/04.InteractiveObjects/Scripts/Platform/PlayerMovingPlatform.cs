using UnityEngine;

namespace LabLuby.Platform
{
    public class PlayerPlatform : MonoBehaviour
    {
        [SerializeField] private Transform platform;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        [SerializeField] private float speed = 1f;
        int direction = 1;

        private void Update()
        {
            Vector2 target = CurrentMovementTarget();
            platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);


            //float distance = (target - (Vector2)platform.position).magnitude;


            /*
            Vector2 target = CurrentMovementTarget();
            platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

            float distance = (target - (Vector2)platform.position).magnitude;
            if (distance < 0.1f)
            {
                direction *= -1;
            }
            */
        }

        Vector2 CurrentMovementTarget()
        {
            if (direction == 1)
            {
                return startPoint.position;
            }
            else
            {
                return endPoint.position;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                direction = -1;
                //collision.gameObject.CompareTag("Player") = transform;
                GameObject.FindWithTag("Player").transform.parent = transform;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                direction = 1;
                //GameObject.FindWithTag("Player").transform.parent = null;
                GameObject.FindWithTag("Player").transform.parent = null;
            }
        }



      


    }

}
