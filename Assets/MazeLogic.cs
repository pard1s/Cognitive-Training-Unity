using UnityEngine;
using UnityEngine.UI;

public class MazeGameManager : MonoBehaviour
{
    public static MazeGameManager Instance;

    public GameObject startMenu; // Start Menu for Maze Game
    public GameObject finishMenu; // Finish Menu for Maze Game
    public GameObject mazeGameView; // Game view for Maze Game
    public Button startMazeButton; // Button to start the maze game
    public Button startButton; // Button in the start menu to start the game
    public Button replayButton; // Button in the finish menu to replay the game

    public Camera mainCamera; // Reference to Main Camera

    private Vector3 defaultCameraPosition;
    private float defaultOrthographicSize;
    private Camera newCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Ensure initial states
        startMenu.SetActive(false);
        finishMenu.SetActive(false);
        mazeGameView.SetActive(false);

        // Add listeners to buttons
        startMazeButton.onClick.AddListener(OnStartMazeButtonClicked);
        startButton.onClick.AddListener(OnStartButtonClicked);
        replayButton.onClick.AddListener(OnReplayButtonClicked);
    }

    public void OnStartMazeButtonClicked()
    {
        // Deactivate the main camera
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(false);
        }


        // Create a new camera
        newCamera = new GameObject("NewCamera").AddComponent<Camera>();
        newCamera.transform.position = new Vector3(0, 0, -10); // Set the position to (0, 0, -10)
        newCamera.orthographic = true;
        newCamera.orthographicSize = 16.5f;

        startMenu.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        startMenu.SetActive(false);
        mazeGameView.SetActive(true);
    }

    public void OnReplayButtonClicked()
    {
        finishMenu.SetActive(false);
        mazeGameView.SetActive(true);
    }

    public void onExitButtonClicked()
    {
        // Reactivate the main camera
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(true);
        }

        // Destroy the new camera
        if (newCamera != null)
        {
            Destroy(newCamera.gameObject);
            Destroy(newCamera.gameObject);
            newCamera = null; // Clear the reference
        }
        startMenu.SetActive(false);
        finishMenu.SetActive(false);
        mazeGameView.SetActive(false);
    }

    public void GameFinished()
    {
        mazeGameView.SetActive(false);
        finishMenu.SetActive(true);
    }
}
