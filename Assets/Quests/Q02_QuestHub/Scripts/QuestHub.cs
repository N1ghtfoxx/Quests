using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuestHub : MonoBehaviour
{
    public string hubSceneName = "Q02_QuestHub"; // Name of the hub scene

    private InputSystem_Actions inputActions;

    // initialize input actions
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    // enable input actions when the object is enabled
    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Cancel.performed += OnCancel;
    }

    // disable input actions when the object is disabled
    private void OnDisable()
    {
        inputActions.UI.Cancel.performed -= OnCancel;
        inputActions.UI.Disable();
    }

    // method called when the Cancel action is performed
    private void OnCancel(InputAction.CallbackContext context)
    {
        // Load the hub scene when 'ESC' is pressed
        LoadHubScene();
    }

    // method to load the hub scene
    public void LoadHubScene()
    {
        SceneManager.LoadScene(hubSceneName);
    }

    // method to load Quest 1 scene (can be duplicated for other quests)
    public void LoadQuest1Scene()
    {
               SceneManager.LoadScene("Q01_ClickGrow");
    }
}