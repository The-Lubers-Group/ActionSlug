using UnityEngine;

namespace LabLuby.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {

        [SerializeField] protected int maxHealth = 100;
        protected int currentHealth;

        [Range(1f, 8f)]
        [SerializeField] protected float speed;
        protected GameObject player;
        protected Animator anim;


        private void Awake()
        {
            currentHealth = maxHealth;
            //speed = Random.value;
            speed = Random.Range(1f, 8f);
        }

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void TakeDamage(int damage)
        {
            Debug.Log(currentHealth);
            currentHealth -= maxHealth;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Morreu");
            GameObject.Destroy(gameObject);
        }
    }

}
