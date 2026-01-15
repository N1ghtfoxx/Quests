using UnityEngine;

namespace Quests.Q04
{
    public class Bullet : MonoBehaviour
    {
        private Vector2 velocity;

        public void Initialize(Vector2 direction, float speed)
        {
            velocity = direction * speed;

            // rotate bullet asset towards movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        void Update()
        {
            transform.position += (Vector3)(velocity * Time.deltaTime);
        }

        // destroy bullet when it goes off-screen
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}