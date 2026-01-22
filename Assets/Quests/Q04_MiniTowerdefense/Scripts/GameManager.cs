using UnityEngine;

namespace Quests.Q04
{
    public class GameManager : MonoBehaviour
    {
        // referenz on GameOver panel in canvas
        [SerializeField] private GameObject gameOverPanel;

        // Enemy prefab to spawn
        [SerializeField] private GameObject enemyPrefab;

        // Time interval between enemy spawns
        [SerializeField] private float spawnInterval = 2f;

        private float spawnTimer;
        private bool isGameOver = false; // so no enemy spawns, wenn game over

        // Start is called before the first frame update
        void Start()
        {
            spawnTimer = spawnInterval;

            // make sure panel is off 
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // if game over, don't do stuff
            if (isGameOver)  return; 

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
            }
        }

        // this method is called by Enemy
        public void TriggerGameOver()
        {
            if(isGameOver) return;

            isGameOver = true;

            // stopp playtime
            Time.timeScale = 0f;

            // show GameOver screen
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            Debug.Log("Game Over!");
        }

        #region Enemy Spawning

        // Method to spawn an enemy randomly 
        // at top right bottom or left side of the screen
        void SpawnEnemy()
        {
            Camera cam = Camera.main;

            // distance from camera to z=0 plane
            float distance = Mathf.Abs(cam.transform.position.z); 

            // randomly choose a side: 0 = top, 1 = right, 2 = bottom, 3 = left
            int side = Random.Range(0, 4);

            Vector3 screenPos = Vector3.zero;

            if (side == 0)         // top
                screenPos = new Vector3(Random.Range(0, Screen.width), Screen.height, cam.nearClipPlane);
            else if (side == 1)    // right
                screenPos = new Vector3(Screen.width, Random.Range(0, Screen.height), cam.nearClipPlane);
            else if (side == 2)    // bottom
                screenPos = new Vector3(Random.Range(0, Screen.width), 0, cam.nearClipPlane);
            else if (side == 3)    // left
                screenPos = new Vector3(0, Random.Range(0, Screen.height), cam.nearClipPlane);

            // convert screen position to world position
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);

            // set z to 0 (assuming a 2D plane at z=0)
            worldPos.z = 0f;

            Instantiate(enemyPrefab, worldPos, Quaternion.identity); // spawn enemy at calculated position
        }

        #endregion
    }
}
