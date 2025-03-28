using Interfaces;
using PlayerStates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour , IDamageable
{
    // Events
    public event UnityAction OnPlayerDeath;
    public event UnityAction<float> OnParry;
    public event UnityAction<float, float> OnHit;
    
    [Header("References")]
    public Rigidbody rb;
    public Animator animator;
    public Transform cameraTransform;
    public AnimationClip attackAnimation;
    public AnimationClip blockAnimation;
    public Weapons.Weapon weapon;
    [SerializeField] private LevelUpSystem levelUpSystem;
    
    [Header("Forces")]
    public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    public float speedRotation = 10f;

    [Header("Stats")] 
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float damageReduction = 0.8f; // Should be between 0-1
    
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;

    [Header("Durations")] 
    public float parryWindow = 0.5f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private float _curHp;
    
    // Player state
    private PlayerState _currentState;
    
    // Input controls
    private PlayerControls _controls;
    
    // Input Action
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _attackAction;
    private InputAction _blockAction;
    
    // Move input and is dashing bool
    public bool IsLockedOn { get; set; }
    private Vector2 _moveInput;
    private bool _isDashing;
    
    // Damage Multiplier
    private float _damageMultiplier = 1f;
    
    // Defence booleans
    public bool IsParrying { get; set; }
    public bool IsBlocking { get; set; }

    private void Awake()
    {
        _controls = new PlayerControls();

        _moveAction = _controls.Player.Move;
        _jumpAction = _controls.Player.Jump;
        _dashAction = _controls.Player.Dash;
        _attackAction = _controls.Player.Attack;
        _blockAction = _controls.Player.Block;
    }
    
    private void OnEnable()
    {
        _controls.Enable();
        levelUpSystem.OnLevelUp += LevelUp;
    }

    private void OnDisable()
    {
        _controls.Disable();
        levelUpSystem.OnLevelUp -= LevelUp;
    }

    private void Start()
    {
        _curHp = maxHp;
        _currentState = new IdleState(this, cameraTransform);
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

    public bool BlockPressed()
    {
        return _blockAction.triggered;
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
    
    private void LevelUp()
    {
        weapon.damage += 10f;
    }

    public void EnableWeaponCollider()
    {
        weapon.EnableCollider();
    }

    public void DisableWeaponCollider()
    {
        weapon.DisableCollider();
    }
    
    // IDamagable function
    public void TakeDamage(float damage)
    {
        if (IsParrying)
        {
            _damageMultiplier += 0.25f;
            weapon.SetDamageWithMultiplier(_damageMultiplier);
            OnParry?.Invoke(_damageMultiplier);
            return;
        }

        if (IsBlocking)
        {
            _curHp -= damage * damageReduction;
        }
        else
        {
            _curHp -= damage;
        }
        
        // Reset damage multiplier
        _damageMultiplier = 1f;
        weapon.SetDamageWithMultiplier(_damageMultiplier);
        
        // Invoke On Hit event
        OnHit?.Invoke(_curHp, maxHp);

        if (_curHp <= 0f)
        {
            _curHp = 0f;
            // TODO: Trigger Death Event
            OnPlayerDeath?.Invoke();
        }
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundRadius);
    }
}
