using PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Animator animator;
    public Transform cameraTransform;
    public AnimationClip attackAnimation;
    
    [Header("Forces")]
    public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    public float speedRotation = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")] 
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    
    // Player state
    private PlayerState _currentState;
    
    // Input controls
    private PlayerControls _controls;
    
    // Input Action
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _attackAction;
    
    // Move input and is dashing bool
    private Vector2 _moveInput;
    private bool _isDashing;

    private void Awake()
    {
        _controls = new PlayerControls();

        _moveAction = _controls.Player.Move;
        _jumpAction = _controls.Player.Jump;
        _dashAction = _controls.Player.Dash;
        _attackAction = _controls.Player.Attack;
    }
    
    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        _currentState = new IdleState(this, cameraTransform, Vector2.zero);
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.HandleInput();
        _currentState.UpdateState();
    }
    
    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }
    
    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }

    public void TransitionToState(PlayerState newState)
    {
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }

    public bool MovePressed()
    {
        return _moveAction.triggered || _moveAction.inProgress;
    }
    
    public bool JumpPressed()
    {
        return _jumpAction.triggered;
    }
    
    public bool DashPressed()
    {
        return _dashAction.triggered;
    }
    
    public bool AttackPressed()
    {
        return _attackAction.triggered;
    }
    
    // This function called by the animator - Jump Animation event.
    public void OnJumpAnimation()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }
    
    // Read input reading for movement.
    public Vector2 GetMoveInput()
    {
        return _controls.Player.Move.ReadValue<Vector2>();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundRadius);
    }
}
