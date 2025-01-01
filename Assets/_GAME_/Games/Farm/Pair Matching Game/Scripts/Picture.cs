using UnityEngine;
using System.Collections;

public class Picture : MonoBehaviour
{
    private Material _firstMaterial;
    private Material _secondMaterial;

    private Quaternion _currentRotation;

    [HideInInspector]
    public bool Revealed = false;
    private PictureManager _pictureManager;
    private bool _clicked = false;
    private int _index;

    public void SetIndex(int id) { _index = id; }

    public int GetIndex() { return _index; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revealed = false;
        _clicked = false;
        _pictureManager = GameObject.Find("[PictureManager]").GetComponent<PictureManager>();
        _currentRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (_clicked == false)
        {
            _pictureManager.CurrentPuzzleState = PictureManager.PuzzleStates.PuzzleRotating;
            StartCoroutine(LoopRotation(45, false));
            _clicked = true;
        }
    }

    public void FlipBack() { 
        if(gameObject.activeSelf)
        {
            _pictureManager.CurrentPuzzleState = PictureManager.PuzzleStates.PuzzleRotating;
            Revealed = false;
            StartCoroutine(LoopRotation(45, true));
        }
    }

    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rot = 0f;
        const float dir = 1f;
        const float rotSpeed = 180f;
        const float rotSpeed1 = 90f;
        var startAngle = angle;
        var assigned = false;

        if (FirstMat)
        {
            while (rot < angle)
            {
                var step = Time.deltaTime * rotSpeed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                if (rot >= (startAngle - 2) && assigned == false)
                {
                    ApplyFirstMaterial();
                    assigned = true;

                }

                rot += (1 * step * dir);
                yield return null;
            }
        }
        else
        {
            while(angle>0)
            {
                float step = Time.deltaTime * rotSpeed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                angle -= (1 * step * dir);

                yield return null;
            }
        }

        gameObject.GetComponent<Transform>().rotation = _currentRotation;

        if (!FirstMat)
        {
            Revealed = true;
            ApplySecondMaterial();
            _pictureManager.CheckPicture();
        }
        else
        {
            _pictureManager.PuzzleRevealedNumber = PictureManager.RevealedState.NoRevealed;
            _pictureManager.CurrentPuzzleState = PictureManager.PuzzleStates.CanRotate;
        }
        _clicked = false;
    }

    public void SetFirstMaterial(Material mat, string texturePath)
    {
        _firstMaterial = mat;
        _firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }
    
    public void SetSecondMaterial(Material mat, string texturePath)
    {
        _secondMaterial = mat;
        _secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void RevealTemporarily()
{
    ApplySecondMaterial(); // Show the paired image
    Revealed = false; // Ensure it’s not flagged as permanently revealed
}

    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _firstMaterial;
    }
    
    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _secondMaterial;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateCorutine());
    }

    private IEnumerator DeactivateCorutine()
    {
        Revealed = false;

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
