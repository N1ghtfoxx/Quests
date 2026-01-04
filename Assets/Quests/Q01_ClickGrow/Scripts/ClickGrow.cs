using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class ClickGrow : MonoBehaviour
{
    [SerializeField] private float growAmount = 0.2f;
    [SerializeField] private LayerMask layerMask;
    private bool showRaycastDebug = true;
    private float raycastLength = 100.0f;
    private Camera mainCamera;
    private PlayerInput playerInput;
    private IGrowable lastClicked;

    // Initialize main camera and player input
    private void Start()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
    }

    /// <summary>
    ///  this method performs a raycast from the mouse position to detect growable objects
    /// </summary>
    private bool TryGetGrowableObject(out IGrowable growable)
    {
        growable = null;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength, layerMask))
        {
            // visual debug for raycast
            if (showRaycastDebug)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green, 0.1f);
                Debug.Log($"Raycast Hit: {hit.collider.gameObject.name}");
            }

            // check if hit object has IGrowable component
            if (hit.collider.TryGetComponent<IGrowable>(out IGrowable growableComponent))
            {
                growable = growableComponent;
                return true;
            }
        }
        else
        {
            // if raycast misses, draw red line for debug
            if (showRaycastDebug)
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * raycastLength, Color.red, 0.1f);
            }
        }

        return false;
    }

    // grow an object when left MouseButton clicked
    public void OnGrow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (TryGetGrowableObject(out IGrowable growable))
            {
                lastClicked = growable;
                growable.Grow(growAmount);
                Debug.Log("Object grew.");
            }
            else
            {
                Debug.Log("No growable object under cursor.");
            }
        }
    }

    // grow ast clicked object just on the x axis when 'X' is pressed
    public void OnGrowX(InputAction.CallbackContext context)
    {

        if (!context.performed) return;
        if(lastClicked == null) return;
        Debug.Log("OnGrowX called.");

        var go = lastClicked.GetGameObject();
        go.transform.localScale += new Vector3(growAmount, 0, 0);
    }

    //grow last clicked object just on the z axis when 'Z' is pressed
    public void OnGrowZ(InputAction.CallbackContext context)
    {

        if (!context.performed) return;
        if (lastClicked == null) return;
        Debug.Log("OnGrowZ called.");

        var go = lastClicked.GetGameObject();
        go.transform.localScale += new Vector3(0, 0, growAmount);
    }

    // reset last clicked object to original size when 'R' is pressed
    public void OnReset(InputAction.CallbackContext context)
    {

        if (!context.performed) return;
        if (lastClicked == null) return;
        Debug.Log("OnReset called.");

        var go = lastClicked.GetGameObject();
        go.transform.localScale = Vector3.one;
    }
}

// interface for growable objects
public interface IGrowable
{
    void Grow(float amount);
    Vector3 GetCurrentScale();
    GameObject GetGameObject();
}