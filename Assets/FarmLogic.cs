using UnityEngine;
using UnityEngine.UI;

public class FarmLogic : MonoBehaviour
{
    //public GameObject MGTrigger; // Reference to MGTrigger
    public GameObject BTNMatchingGameStart; // Button to start Matching Game
    public GameObject MatchingGameStartMenu; // Start Menu for Matching Game
    public GameObject MatchingGameExitMenu; // Start Menu for Matching Game
    public GameObject Game; // Game object

    public Button BTNPlay; // Play Button reference
    public Button BTNMatchingGameStartButton; // Start Matching Button reference

    public Camera mainCamera; // Reference to Main Camera

    private Vector3 defaultCameraPosition;
    private float defaultOrthographicSize;
    private Camera newCamera;

    private void Start()
    {
        // Ensure initial states
        //BTNMatchingGameStart.SetActive(false);
        MatchingGameStartMenu.SetActive(false);
        MatchingGameExitMenu.SetActive(false);
        Game.SetActive(false);

        // Add listeners to buttons
        BTNMatchingGameStartButton.onClick.AddListener(OnStartMatchingGameClicked);
        BTNPlay.onClick.AddListener(OnPlayButtonClicked);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("Triggered");
    //    if (other.CompareTag("Player")) // Ensure the object entering is the Player
    //    {
    //        print("Player Entered");
    //        BTNMatchingGameStart.SetActive(true);
    //    }
    //}

    public void OnStartMatchingGameClicked()
    {
        BTNMatchingGameStart.SetActive(false);
        MatchingGameStartMenu.SetActive(true);
    }

    private void OnPlayButtonClicked()
    {
        MatchingGameStartMenu.SetActive(false);
        Game.SetActive(true);

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
    }

    public void OnExitButtonClicked()
    {
        MatchingGameStartMenu.SetActive(false);
        MatchingGameExitMenu.SetActive(false);
        Game.SetActive(false);
        BTNMatchingGameStart.SetActive(true);
        var pictureManager = FindObjectOfType<PictureManager>();
        if (pictureManager != null)
        {
            pictureManager.ResetGame();
        }
        // Reactivate the main camera
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(true);
        }

        // Destroy the new camera
        if (newCamera != null)
        {
            Destroy(newCamera.gameObject);
        }
    }

    public void onReplayButtonClicked()
    {
        var pictureManager = FindObjectOfType<PictureManager>();
        if (pictureManager != null)
        {
            pictureManager.ResetGame();
        }
        MatchingGameExitMenu.SetActive(false);
        Game.SetActive(true);
    }

    public void GameFinished()
    {
        Game.SetActive(false);
        MatchingGameExitMenu.SetActive(true);
    }
}


