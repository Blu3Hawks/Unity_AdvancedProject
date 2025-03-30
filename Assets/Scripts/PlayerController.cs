using Interfaces;
using PlayerStates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;

public class PlayerController : MonoBehaviour, IDamageable, IDataPersistence
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
    public Weapon weapon;
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

    public float _curHp;

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
    public Vector3 entryPointPosition;//hen's logic

    // Damage Multiplier
    private float _damageMultiplier = 1f;

    // Defence booleans
    public bool IsParrying { get; set; }
    public bool IsBlocking { get; set; }
    public bool IsDead { get; private set; } = false;
    public LevelUpSystem LevelUpSystem { get { return levelUpSystem; } }

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
        TakeDamage(0); //just to reset these things
        //hen's logic
        if (DataPersistenceManager.instance.GameData.PlayerPosition != Vector3.zero)
        {
            transform.position = DataPersistenceManager.instance.GameData.PlayerPosition;
        }
        else
        {
            transform.position = entryPointPosition;
        }
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

    // Level up your weapon dmg
    private void LevelUp(int curLevel)
    {
        weapon.SetBaseDamage(5f);
    }

    // This function called by the animator - Attack Animation event.
    public void EnableWeaponCollider()
    {
        weapon.EnableCollider();
    }

    // This function called by the animator - Attack Animation event.
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
            IsDead = true;
            OnPlayerDeath?.Invoke();
        }
    }

    // Draw the sphere that checks for ground check
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundRadius);
    }

    //hen's logic
    public void LoadData(GameData data)
    {
        if (data.PlayerPosition != Vector3.zero)
        {
            transform.position = data.PlayerPosition;
        }
        else
        {
            transform.position = entryPointPosition;
        }
        _curHp = DataPersistenceManager.instance.GameData.playerCurrentHealth;
        Debug.Log(DataPersistenceManager.instance.GameData.playerCurrentHealth+ ", " + _curHp);
        levelUpSystem.CurXp = DataPersistenceManager.instance.GameData.playerCurrentXp;
        levelUpSystem.CurrentLevel = DataPersistenceManager.instance.GameData.playerLevel;
    }

    public void SaveData(GameData data)
    {
        DataPersistenceManager.instance.GameData.PlayerPosition = transform.position;
        DataPersistenceManager.instance.GameData.playerCurrentHealth = _curHp;
        DataPersistenceManager.instance.GameData.playerCurrentXp = levelUpSystem.CurXp;
        DataPersistenceManager.instance.GameData.playerLevel = levelUpSystem.CurrentLevel;
    }

    private void OnApplicationQuit()
    {
        SaveData(null);
        DataPersistenceManager.instance.GameData.PlayerPosition = transform.position;
        Debug.Log(DataPersistenceManager.instance.GameData.PlayerPosition);
    }

    public void SetEntryPointAndCamera(Vector2 entryPoint, Transform cameraTransform)
    {
        entryPointPosition = new Vector3(entryPoint.x, transform.position.y + 1, entryPoint.y + 2);  // Adjusted to use the correct y position
        transform.position = entryPointPosition;
        this.cameraTransform = cameraTransform;
    }
}
