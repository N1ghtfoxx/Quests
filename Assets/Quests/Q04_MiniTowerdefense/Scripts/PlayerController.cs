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

        // player needs to look towards mouse position
        private void Update()
        {
            Vector3 mousePosition = inputActions.Q04.Look.ReadValue<Vector2>();
            Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));
            Vector3 direction = new Vector3(worldMousePosition.x - transform.position.x, 0, worldMousePosition.z - transform.position.z);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}