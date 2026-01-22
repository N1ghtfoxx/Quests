using UnityEngine;

namespace Quests.Q04
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;

        private Transform player; 

        void Start()
        {
            // search for game object with tag "Player" 
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            // if found, store its transform
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        void Update()
        {
            if (player == null) return;

            // direction towards player
            Vector2 direction = (player.position - transform.position).normalized;

            // movement towards player
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                GameManager gm = FindFirstObjectByType<GameManager>();

                if (gm != null)
                {
                    gm.TriggerGameOver();
                }

                // destroy player
                Destroy(other.gameObject);

                // destroy enemy on collision with player
                Destroy(gameObject);
            }
        }
    }
}
