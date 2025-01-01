using UnityEngine;

[SelectionBase]
public class ChickenController : MonoBehaviour
{
    private enum Directions
    {
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    #region Editor Data 
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;
    [SerializeField] float _walkingDuration = 4f;

    [Header("Dependencies")] // This will create a header in the inspector.
    [SerializeField] Rigidbody2D _rb;   // == private Rigidbody2d myRigidBody;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    #region Internal Data // This region will group the variables that are related to the internal data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.Right; // right is the default direction
    private float _stateTimer;
    private readonly int _animMoveRight = Animator.StringToHash("AA_Chicken_Right"); // the hash of the move right animation
    private readonly int _animMoveLeft = Animator.StringToHash("AA_Chicken_Left"); // the hash of the move right animation
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveChicken();

    }

    // Update is called once per frame
    void Update()
    {
        _stateTimer -= Time.fixedDeltaTime;
        if (_stateTimer < 0)
        {
            ChangeDirection();
        }
        MoveChicken();
    }

    private void ChangeDirection()
    {
        _facingDirection = (Directions)Random.Range(0, 6);
        _stateTimer = Random.Range(_walkingDuration - 2, _walkingDuration + 2);
        switch (_facingDirection)
        {
            case Directions.Left:
                _moveDir = Vector2.left;
                _animator.CrossFade(_animMoveLeft, 0);
                break;
            case Directions.Right:
                _moveDir = Vector2.right;
                _animator.CrossFade(_animMoveRight, 0);
                break;
            case Directions.UpLeft:
                _moveDir = new Vector2(-1, 1).normalized;
                _animator.CrossFade(_animMoveLeft, 0);
                break;
            case Directions.UpRight:
                _moveDir = new Vector2(1, 1).normalized;
                _animator.CrossFade(_animMoveRight, 0);
                break;
            case Directions.DownLeft:
                _moveDir = new Vector2(-1, -1).normalized;
                _animator.CrossFade(_animMoveLeft, 0);
                break;
            case Directions.DownRight:
                _moveDir = new Vector2(1, -1).normalized;
                _animator.CrossFade(_animMoveRight, 0);
                break;
        }
    }
    private void MoveChicken()
    {
        _rb.linearVelocity = _moveDir * _moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection();
    }
}
