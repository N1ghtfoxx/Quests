using System.Runtime.InteropServices;
using UnityEngine;

namespace Quests.Q04
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;

        private InputSystem_Actions inputActions;
        private Camera mainCamera;

        private void Awake()
        {
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            inputActions.Q04.Enable();
        }

        private void OnDisable()
        {
            inputActions.Q04.Disable();
        }

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            // check for shoot input
            if (inputActions.Q04.Fire.WasPerformedThisFrame())
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            // get mouse position in world space
            Vector2 mouseScreenPos = inputActions.Q04.Look.ReadValue<Vector2>();

            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(
                new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f)
            );

            // calculate direction from player to mouse position
            Vector2 direction = (mouseWorldPos - transform.position).normalized;

            // spawn bullet at player's position
            GameObject bulletObj = Instantiate(
                bulletPrefab, 
                transform.position, 
                Quaternion.identity
            );

            // hand off direction and speed to bullet script
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.Initialize(direction, bulletSpeed);
        }
    }
}
