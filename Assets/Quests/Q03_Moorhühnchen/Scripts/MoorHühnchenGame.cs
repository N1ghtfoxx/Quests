using UnityEngine;
using TMPro;
//using System.Collections.Generic;
//using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;


namespace Quests.Q03
{
    public class MoorHühnchenGame : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private float targetLifetime = 3f;

        [Header("Spawn Area")]
        [SerializeField] private Vector2 spawnMargin = new Vector2(0.1f, 0.1f); // Margin from screen edges (in viewport coordinates)

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Input")]
        [SerializeField] private InputActionAsset inputActions;

        [Header("Object Pooling")]
        [SerializeField] private string targetPoolName = "Targets";

        private int score = 0;
        private float spawnTimer = 0f;
        private Camera mainCam;
        private InputAction clickAction;
        private InputAction pointerPositionAction;


        // Start is called before the first frame update
        // this method is called when the script instance is being loaded
        // here, we initialize variables and set up the game
        private void Start()
        {
            mainCam = Camera.main; // main camera reference

            if (mainCam == null) 
            {
                Debug.LogError("Main Camera not found.");
                return;
            }

            if (targetPrefab == null)
            {
                Debug.LogError("Target Prefab is not assigned.");
                return;
            }

            if(inputActions == null)
            {
                Debug.LogError("InputActionAsset is not assigned.");
                return;
            }
            SetupInputActions(); // Enable input actions
            UpdateScoreUI();    // Initialize score UI

            Debug.Log("MoorHühnchen Game Started! Click on targets to score points.");
        }


        // This method finds and enables the necessary input actions for the game
        // It looks for the "Click" and "PointerPosition" actions within the "Q03" action map
        // If the action map or actions are not found, it logs an error
        // Once found, it enables the actions for use in the game
        void SetupInputActions()
        {
            var actionMap = inputActions.FindActionMap("Q03");

            if(actionMap == null)
            {
                if(inputActions.actionMaps.Count > 0)
                {
                    actionMap = inputActions.actionMaps[0];
                    Debug.Log($"Using Action Map; {actionMap.name}");
                }
                else
                {
                    Debug.LogError("No action maps found in InputSystem_Actions!");
                    return;
                }
            }
            clickAction = actionMap.FindAction("Click");
            pointerPositionAction = actionMap.FindAction("PointerPosition");

            if(clickAction == null)
            {
                Debug.LogError("Click action not found in InputSystem_Actions!");
                foreach(var action in actionMap.actions)
                {
                    Debug.Log($" - {action.name}");
                }
                return;
            }
            clickAction.Enable();
            pointerPositionAction.Enable();

            Debug.Log("Input Actions enabled successfully.");
        }

        // Update is called once per frame
        // This method handles the spawning of targets and user input 
        private void Update()
        {
            HandleSpawning();
            HandleInput();
        }


        // This method manages the timing for spawning targets
        // It increments the spawn timer by the time elapsed since the last frame
        // When the spawn timer exceeds the defined spawn interval, it spawns a new target and resets the timer
        void HandleSpawning()
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnTarget();
                spawnTimer = 0f;
            }
        }

        // This method spawns a target at a random position on the screen
        // gets object from an object pool
        // and spawns object at random position on screen
        private void SpawnTarget()
        {
            Vector3 spawnPos = GetRandomScreenPosition();  // Get a random position within the screen bounds

                //GameObject target = Instantiate(targetPrefab, spawnPos, Quaternion.identity); // Instantiate the target prefab (Quaternion.identity means no rotation)
            
            //get object from pool instead of instantiate
            GameObject target = ObjectPoolManager.Instance.Get(targetPoolName);

            if (target == null)
            {
                Debug.LogWarning("No target available in pool!");
                return;
            }
            target.transform.position = spawnPos;
            target.SetActive(true);

            Target targetScript = target.GetComponent<Target>(); 
            if (targetScript == null)
            {
                targetScript = target.AddComponent<Target>();
            }
            targetScript.Initialize(this, targetLifetime);

            Debug.Log("Target spawned at: " + spawnPos);
        }

        // This method calculates a random position on the screen within defined margins
        // It uses the camera's viewport to world point conversion to get the correct position in the game world
        private Vector3 GetRandomScreenPosition()
        {
            float x = Random.Range(spawnMargin.x, 1f - spawnMargin.x); // Random x position within margins
            float y = Random.Range(spawnMargin.y, 1f - spawnMargin.y); // Random y position within margins

            Vector3 viewportPos = new Vector3(x, y, 10f); // z=10 to ensure it's in front of the camera
            Vector3 worldPos = mainCam.ViewportToWorldPoint(viewportPos); // Convert to world position
            worldPos.z = 0f; // Set z to 0 for 2D game

            return worldPos;
        }

        // This method handles user input for clicking on targets
        // It checks if the click action was performed in the current frame
        // If so, it reads the pointer position and converts it to world coordinates
        // It performs a raycast to check if a target was hit
        void HandleInput()
        {
            if(clickAction.WasPerformedThisFrame())
            {
                Vector2 screenPos = pointerPositionAction.ReadValue<Vector2>();
                Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);

                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit.collider != null)
                {
                    Target target = hit.collider.GetComponent<Target>(); // Check if the hit object has a Target component
                    if (target != null)
                    {
                        OnTargetHit(target.gameObject); // Handle target hit
                    }
                }
                else
                {
                    Debug.Log("Missed! No target hit.");
                }
            }
        }

        // This method is called when a target is hit by the player
        // It increments the score, updates the score UI, logs the hit, and puts the target back into an object pool instead of destroying it
        public void OnTargetHit(GameObject target)
        {
            score++;
            UpdateScoreUI();

            Debug.Log("Target Hit! Score: " + score);

            //Destroy(target);

            // back to pool instead of destroy 
            target.SetActive(false);
        }

        // This method is called when a target expires (is not hit in time)
        // It logs the expiration and puts the target back into an object pool instead of destroying it
        public void OnTargetExpired(GameObject target)
        {
            Debug.Log("Target expired (missed)");
            //Destroy(target);

            // back to pool instead of destroy 
            target.SetActive(false);

        }

        // This method updates the score display in the UI
        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
            else
            {
                Debug.Log($"Current Score: {score}");
            }
        }
        
        // no longer needed, because object pooling is used 
        //// This method is called when the game object is destroyed
        //// It disables the input actions to clean up resources
        //void OnDestroy()
        //{
        //    if(clickAction != null)
        //        clickAction.Disable();
        //    if(pointerPositionAction != null)
        //        pointerPositionAction.Disable();
        //}
    }

    // This class represents a target in the MoorHühnchen game
    public class Target : MonoBehaviour
    {
        private MoorHühnchenGame game; // Reference to the main game script
        private float lifetime;
        private float age = 0f;

        // This method initializes the target with a reference to the game and its lifetime
        // It also ensures the target has a Collider2D component for hit detection
        public void Initialize(MoorHühnchenGame gameInstance, float targetLifetime) 
        {
            game  = gameInstance;
            lifetime = targetLifetime;
            age = 0f;

            if(GetComponent<Collider2D>() == null)
            {
                BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
                Debug.Log("Added BoxCollider2D to Target.");
            }
        }

        // Update is called once per frame
        // This method tracks the age of the target and checks if it has exceeded its lifetime
        private void Update()
        {
            age += Time.deltaTime;

            if (age >= lifetime)
            {
                if (game != null)
                {
                    game.OnTargetExpired(gameObject);
                }
            }
        }
    }
}
