using System;
using UnityEngine;

namespace Quests.Q04
{
    public class PlayerController : MonoBehaviour
    {
        private InputSystem_Actions inputActions;
        private Camera mainCamera;

        private void Start()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Q04.Enable();
            mainCamera = Camera.main;
        }

        // in this method, we will rotate the player to face the mouse position
        private void Update()
        {
            // get mouse position from input system
            Vector3 mouseScreenPos = inputActions.Q04.Look.ReadValue<Vector2>();

            // convert to world position
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(
                new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f)
            );

            // calculate direction from player to mouse position (xy plane only)
            Vector2 direction = (mouseWorldPos - transform.position);

            // calculate angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // set player rotation (subtract 90 degrees if the player sprite points up)
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }
}