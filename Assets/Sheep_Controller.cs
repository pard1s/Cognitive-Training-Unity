using UnityEngine;

[SelectionBase] // This attribute will make the GameObject be selected by clicking on the script in the Inspector
public class Sheep_Controller : MonoBehaviour
{
    #region Enums
    private enum Directions
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
    private enum SheepState
    {
        Walking,
        Eating,
        Sleeping
    }
    #endregion

    #region Editor Data // This region will group the variables that are related to the editor data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 80f;
    [SerializeField] float _walkingDuration = 4f;
    [SerializeField] float _sleepDuration = 6f;
    [SerializeField] float _eatingDuration = 1f;
    
    [Header("Dependencies")] // This will create a header in the inspector.
    [SerializeField] Rigidbody2D _rb;   // == private Rigidbody2d myRigidBody;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    #region Internal Data // This region will group the variables that are related to the internal data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.Right; // right is the default direction
    private SheepState _currentState = SheepState.Sleeping;
    private int _currentStateIdx = 2;
    private float _stateTimer;
    private readonly int _animMoveRight = Animator.StringToHash("AA_Sheep_Walk"); // the hash of the move right animation
    private readonly int _animMoveUp = Animator.StringToHash("AA_Sheep_Walk_UP");
    private readonly int _animMoveDown = Animator.StringToHash("AA_Sheep_Walk_Down");
    private readonly int _animEat = Animator.StringToHash("AA_Sheep_Eat");
    private readonly int _animSleep = Animator.StringToHash("AA_Sheep_Sleep");
    private float[] _durationsArr;
    private int slpChnce;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _durationsArr = new float[] { _walkingDuration, _sleepDuration, _eatingDuration};
        SetRandomState();
    }

    // Update is called once per frame
    void Update()
    {
        _stateTimer -= Time.fixedDeltaTime;
        if(_stateTimer < 0)
        {
            ChangeState();
        }
        switch (_currentState){
            case SheepState.Walking:
                MoveSheep();
                UpdateWalkingAnimation();
                break;
            case SheepState.Eating:
                _animator.CrossFade(_animEat, 0);
                break;
            case SheepState.Sleeping:
                _animator.CrossFade(_animSleep, 0);
                break;
        }
        
    }
    private void SetRandomState()
    {
        _currentStateIdx = (int)Random.Range(0, 3);
        _currentState = (SheepState)_currentStateIdx;
        _stateTimer = Random.Range(_durationsArr[_currentStateIdx]-1, _durationsArr[_currentStateIdx] + 1);

        if(_currentState == SheepState.Walking)
        {
            SetRandomDirection();
        }
    }
    
    private void SetRandomDirection()
    {
        _facingDirection = (Directions)Random.Range(0, 8);
        switch (_facingDirection)
        {
            case Directions.Up:
                _moveDir = Vector2.up;
                _animator.CrossFade(_animMoveUp, 0);
                break;
            case Directions.Down:
                _moveDir = Vector2.down;
                _animator.CrossFade(_animMoveDown, 0);
                break;
            case Directions.Left:
                _moveDir = Vector2.left;
                _animator.CrossFade(_animMoveRight, 0);
                _spriteRenderer.flipX = true;
                break;
            case Directions.Right:
                _moveDir = Vector2.right;
                _animator.CrossFade(_animMoveRight, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.UpLeft:
                _moveDir = (Vector2.up + Vector2.left).normalized;
                _animator.CrossFade(_animMoveUp, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.UpRight:
                _moveDir = (Vector2.up + Vector2.right).normalized;
                _animator.CrossFade(_animMoveUp, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.DownLeft:
                _moveDir = (Vector2.down + Vector2.left).normalized;
                _animator.CrossFade(_animMoveDown, 0);
                _spriteRenderer.flipX = true;
                break;
            case Directions.DownRight:
                _moveDir = (Vector2.down + Vector2.right).normalized;
                _animator.CrossFade(_animMoveDown, 0);
                _spriteRenderer.flipX = false;
                break;
        }
    }
    
    private void ChangeState()
    {
        _currentStateIdx = (_currentStateIdx + 1) % 3;
        if (_currentStateIdx == 2)
        {
            slpChnce = Random.Range(0, 4);
            if (slpChnce != 2){
                _currentStateIdx = (_currentStateIdx + 1) % 3;
            }
        }
        _currentState = (SheepState)_currentStateIdx;
        _stateTimer = Random.Range(_durationsArr[_currentStateIdx] - 1, _durationsArr[_currentStateIdx] + 2);
    }
    
    private void MoveSheep()
    {
        _rb.linearVelocity = _moveDir * _moveSpeed * Time.fixedDeltaTime;
    }

    private void UpdateWalkingAnimation()
    {
        switch (_facingDirection)
        {
            case Directions.Up:
                _animator.CrossFade(_animMoveUp, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.Down:
                _animator.CrossFade(_animMoveDown, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.Left:
                _animator.CrossFade(_animMoveRight, 0);
                _spriteRenderer.flipX = true;
                break;
            case Directions.Right:
                _animator.CrossFade(_animMoveRight, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.UpLeft:
                _animator.CrossFade(_animMoveUp, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.UpRight:
                _animator.CrossFade(_animMoveUp, 0);
                _spriteRenderer.flipX = false;
                break;
            case Directions.DownLeft:
                _animator.CrossFade(_animMoveDown, 0);
                _spriteRenderer.flipX = true;
                break;
            case Directions.DownRight:
                _animator.CrossFade(_animMoveDown, 0);
                _spriteRenderer.flipX = false;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SetRandomDirection();
    }
}
