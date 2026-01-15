using UnityEngine;

namespace Quests.Q04
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;

        private Transform player; 

        void Start()
        {
            // search for game object with name "Player" 
            GameObject playerObj = GameObject.Find("Player");
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
    }
}
