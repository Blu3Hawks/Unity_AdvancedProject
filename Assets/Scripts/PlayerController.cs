using System.Collections;
using PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    
    [Header("Forces")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")] 
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    
    
    private PlayerState _currentState;
    private PlayerControls _controls;
    
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _attackAction;
    
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
        _currentState = new MoveState(this);
        _currentState.EnterState();
    }

    private void Update()
    {
        // Let the current state handle its input and update logic.
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

    // Example attack method.
    public void Attack()
    {
        Debug.Log("Attacking...");
        // Insert attack logic and animation triggers here.
    }

    // Placeholder for attack completion.
    public bool AttackComplete()
    {
        // Replace with actual attack completion condition.
        return true;
    }
    
    // Wrap your input reading for movement.
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
