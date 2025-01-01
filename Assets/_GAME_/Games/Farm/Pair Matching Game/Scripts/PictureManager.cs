using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab; // reference to the picture cube prefab
    public Transform PicSpawnPosition;
    private int numOfRows = 5;
    private int numOfCols = 4;
    private Vector2 startPosition = new Vector2(-6.6f, 5.37f);
    public Transform parent;
    public FarmLogic logic;

    public enum GameState
    {
        NoAction,
        MovingOnPositions,
        DeletingPuzzles,
        FlipBack,
        Checking,
        GameEnd
    };

    public enum PuzzleStates
    {
        PuzzleRotating,
        CanRotate
    }

    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState CurrentGameState;
    [HideInInspector]
    public PuzzleStates CurrentPuzzleState;
    [HideInInspector]
    public RevealedState PuzzleRevealedNumber;

    [HideInInspector]
    public List<Picture> PictureList;
    private Vector2 _offset = new Vector2(4.5f, 4.5f);

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    private int _firstRevealedPic;
    private int _secondRevealedPic;
    private int _revealedPicNumber = 0;
    private int _picToDestroy1;
    private int _picToDestroy2;
    private bool _corutineStarted = false;
    private int destroyedPics = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.Find("[FarmLogic]").GetComponent<FarmLogic>();
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleStates.CanRotate;
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        _revealedPicNumber = 0;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;

        CurrentGameState = GameState.MovingOnPositions;
        LoadMaterials();
        SpawnPictureMesh(numOfRows, numOfCols, startPosition, _offset, false, parent);
        MovePicture(numOfRows, numOfCols, startPosition, _offset);
        // Start the coroutine to show all pictures for 2 seconds
        
    }

    public void CheckPicture()
    {
        CurrentGameState = GameState.Checking;
        _revealedPicNumber = 0;

        for (int id = 0; id < PictureList.Count; id++)
        {
            if (PictureList[id].Revealed && _revealedPicNumber < 2)
            {
                if (_revealedPicNumber == 0)
                {
                    _firstRevealedPic = id;
                    _revealedPicNumber++;

                }
                else if(_revealedPicNumber == 1)
                {
                _secondRevealedPic = id;
                _revealedPicNumber++;
                }
                
            }
        }
        if(_revealedPicNumber == 2)
        {
            if (PictureList[_firstRevealedPic].GetIndex() == PictureList[_secondRevealedPic].GetIndex() && _firstRevealedPic != _secondRevealedPic)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                _picToDestroy1 = _firstRevealedPic;
                _picToDestroy2 = _secondRevealedPic;
            }
            else
            {
                CurrentGameState = GameState.FlipBack;
            }
        }
        else if(_revealedPicNumber >2)
        {
            CurrentGameState = GameState.FlipBack;
            for(int i =0; i < PictureList.Count; i++)
            {
                PictureList[i].FlipBack();
                PictureList[i].Revealed = false;

            }
        }
        CurrentPuzzleState = PictureManager.PuzzleStates.CanRotate;

        if(CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

        // Update is called once per frame
    void Update()
    {
        if(CurrentGameState == GameState.DeletingPuzzles)
        {
            if (CurrentPuzzleState == PuzzleStates.CanRotate)
            {
                DestroyPicture();
            }
        }
        if (CurrentGameState == GameState.FlipBack)
        {
            if(CurrentPuzzleState == PuzzleStates.CanRotate && _corutineStarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }

    }

    private void DestroyPicture()
    {
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        PictureList[_picToDestroy1].Deactivate();
        PictureList[_picToDestroy2].Deactivate();
        _revealedPicNumber = 0;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleStates.CanRotate;
        destroyedPics += 2;
        CheckIfGameFinished();
    }

    private IEnumerator FlipBack()
    {
        //System.Threading.Thread.Sleep(500);
        _corutineStarted = true;

        yield return new WaitForSeconds(0.5f);

        PictureList[_firstRevealedPic].FlipBack();
        PictureList[_secondRevealedPic].FlipBack();
        
        PictureList[_firstRevealedPic].Revealed = false;
        PictureList[_secondRevealedPic].Revealed = false;

        PuzzleRevealedNumber = RevealedState.NoRevealed;
        CurrentGameState = GameState.NoAction;
        _corutineStarted = false;
    }

    private void LoadMaterials()
    {
        var materialFilePath = "Materials/";
        var textureFilePath = "Graphics/PuzzleCat/Vegetables/";
        var pairNumber = 20;
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for (var index = 1; index <= pairNumber; index++) {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            _texturePathList.Add(currentTextureFilePath);
        }

        _firstTexturePath = textureFilePath + firstMaterialName;
        _firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown, Transform p)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PicturePrefab, PicSpawnPosition.position, PicturePrefab.transform.rotation, p);
                tempPicture.name = tempPicture.name + "_C" + col + "_R" + row;
                PictureList.Add(tempPicture);
            }
        }

        ApplyTextures();
    }

    public void ApplyTextures()
    {
        int totalPictures = numOfRows * numOfCols;

        if (totalPictures % 2 != 0)
        {
            Debug.LogError("The total number of pictures must be even to ensure proper pairing.");
            return;
        }

        int pairCount = totalPictures / 2;

        if (_materialList.Count < pairCount)
        {
            Debug.LogError("Not enough materials to create pairs.");
            return;
        }

        // Create and shuffle material indices
        List<int> materialIndices = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            materialIndices.Add(i);
            materialIndices.Add(i);
        }

        // Use Fisher-Yates shuffle for better randomness
        for (int i = materialIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (materialIndices[i], materialIndices[randomIndex]) = (materialIndices[randomIndex], materialIndices[i]);
        }

        // Apply shuffled materials
        for (int i = 0; i < PictureList.Count; i++)
        {
            int materialIndex = materialIndices[i];
            PictureList[i].SetFirstMaterial(_firstMaterial, _firstTexturePath);
            PictureList[i].ApplyFirstMaterial();
            PictureList[i].SetSecondMaterial(_materialList[materialIndex], _texturePathList[materialIndex]);
            PictureList[i].SetIndex(materialIndex);
            PictureList[i].Revealed = false;
        }
    }


    private IEnumerator ShowAllPicturesForSeconds(float seconds)
    {
        // Reveal all pictures
        foreach (var picture in PictureList)
        {
            picture.RevealTemporarily();
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(seconds);

        // Flip all pictures back
        foreach (var picture in PictureList)
        {
            picture.FlipBack();
        }

        // Allow user interaction after flipping back
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleStates.CanRotate;
    }
    private void MovePicture(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (var row = 0; row < rows; row++)
            {
                var targetPosition = new Vector2(pos.x + (offset.x * row), pos.y - (offset.y * col));
                var localPosition = parent.TransformPoint(targetPosition);
                StartCoroutine(MoveToPosition(localPosition, PictureList[index]));
                index++;
            }
        }
        StartCoroutine(ShowAllPicturesForSeconds(2f));
    }

    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 14;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return null;
        }
    }
    public void CheckIfGameFinished()
    {
        bool allDeactivated = true;

        // Check if all pictures are deactivated
        foreach (var picture in PictureList)
        {
            if (picture.gameObject.activeSelf)
            {
                allDeactivated = false;
                break;
            }
        }

        if (destroyedPics == 20 || allDeactivated)
        {
            Debug.Log("Game Finished!");
            GameEnd();
        }
    }

    private void GameEnd()
    {
        CurrentGameState = GameState.GameEnd;
        logic.GameFinished();
    }

    public void ResetGame()
    {
        // Clear existing game objects from the list
        foreach (var picture in PictureList)
        {
            if (picture != null)
            {
                Destroy(picture.gameObject);
            }
        }

        PictureList.Clear(); // Clear the list
        destroyedPics = 0; // Reset destroyed picture count

        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleStates.CanRotate;
        PuzzleRevealedNumber = RevealedState.NoRevealed;

        _revealedPicNumber = 0;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;

        // Reload and restart
        SpawnPictureMesh(numOfRows, numOfCols, startPosition, _offset, false, parent);
        MovePicture(numOfRows, numOfCols, startPosition, _offset);
    }
}