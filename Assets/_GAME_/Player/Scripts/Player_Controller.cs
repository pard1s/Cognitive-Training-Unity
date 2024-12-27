using UnityEngine;

[SelectionBase] // This attribute will make the GameObject be selected by clicking on the script in the Inspector
public class Player_Controller : MonoBehaviour
{
    #region Enums
    private enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    #endregion
    #region Editor Data // This region will group the variables that are related to the editor data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 100f;

    [Header("Dependencies")] // This will create a header in the inspector.
    [SerializeField] Rigidbody2D _rb;   // == private Rigidbody2d myRigidBody;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    #region Internal Data // This region will group the variables that are related to the internal data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.Down; // right is the default direction
    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_002_Move_Right"); // the hash of the move right animation
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_002_Idle_Right");
    private readonly int _animMoveUp = Animator.StringToHash("Anim_Player_002_Move_Up");
    private readonly int _animIdleUp = Animator.StringToHash("Anim_Player_002_Idle_Up");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Player_002_Move_Down");
    private readonly int _animIdleDown = Animator.StringToHash("Anim_Player_002_Idle_Down");
    #endregion

    #region Tick
    private void Update()
    {
        GatherInput(); // Gather the input from the player
        CalculateFacingDirection(); // Calculate the facing direction of the player
        UpdateAnimation();
    }

    private void FixedUpdate() // this function is called at fixed intervals to update player's movement. this is required for physics objects
    {
        MovementUpdate();
    }
    #endregion

    #region Input logic   // This region will group the variables that are related to the input logic
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal"); // Get the horizontal input. GetAxisRaw returns -1, 0 or 1 indicating the direction, while GetAxis returns a value between -1 and 1 indicating the direction and intensity. The "Horizontal" input is defined in the Input Manager. means the arrow keys or the A and D keys.
        _moveDir.y = Input.GetAxisRaw("Vertical"); // Get the vertical input. The "Vertical" input is defined in the Input Manager. means the arrow keys or the W and S keys.

        //print(_moveDir); // Print the move direction to the console, this is the shortcut for Debug.Log(_moveDir); inside the mono behaviour
    }
    #endregion

    #region Movement Logic;
    private void MovementUpdate()
    {
        _rb.linearVelocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;  // the normalized vector will make the vector have a magnitude of 1, so the player will move at the same speed in all directions.
}
    #endregion

    #region Animation Logic
    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
        {
            if (_moveDir.x > 0) // facing right 
            {
                _facingDirection = Directions.Right;
            }
            else if (_moveDir.x < 0) //facing left
            {
                _facingDirection = Directions.Left;
            }
        }
        else if (_moveDir.y != 0)
        {
            if (_moveDir.y > 0) // facing up
            {
                _facingDirection = Directions.Up;
            }
            else if (_moveDir.y < 0) // facing down
            {
                _facingDirection = Directions.Down;
            }
        }
        print(_facingDirection);
    }
    private void UpdateAnimation() {
        if (_facingDirection == Directions.Left)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
        if (_moveDir.SqrMagnitude() > 0) // if any movement button is being pressed
        {
             // crossfade the move right animation, it's a transition effect to go from idle to move with the duration specified in the second parameter
            switch (_facingDirection) {
                case Directions.Right:
                    _animator.CrossFade(_animMoveRight, 0);
                    break;
                case Directions.Left:
                    _animator.CrossFade(_animMoveRight, 0); // Assuming the same animation for left and right
                    break;
                case Directions.Up:
                    _animator.CrossFade(_animMoveUp, 0);
                    break;
                case Directions.Down:
                    _animator.CrossFade(_animMoveDown, 0);
                    break;
            }
        }
        else // if no movement button is being pressed
        {
            switch (_facingDirection)
            {
                case Directions.Right:
                    _animator.CrossFade(_animIdleRight, 0);
                    break;
                case Directions.Left:
                    _animator.CrossFade(_animIdleRight, 0); // Assuming the same animation for left and right
                    break;
                case Directions.Up:
                    _animator.CrossFade(_animIdleUp, 0);
                    break;
                case Directions.Down:
                    _animator.CrossFade(_animIdleDown, 0);
                    break;
            }
        }
    }
    #endregion
}
