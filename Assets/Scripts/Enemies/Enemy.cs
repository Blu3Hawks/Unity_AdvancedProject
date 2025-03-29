using Enemies.States;
using Interfaces;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class Enemy : MonoBehaviour , IDamageable
    {
        // Events
        public event UnityAction<float> OnEnemyDeath;
        
        [Header("References")] 
        public Animator animator;
        public Transform playerTransform;
        public Weapons.Weapon weapon;
        [SerializeField] private HealthBar healthBar;
        
        [Header("Stats")]
        [SerializeField] private float maxHp;
        
        [Header("Forces")]
        public float patrolSpeed = 1f;
        public float chaseSpeed = 2f;
        public float attackDuration = 3f;

        [Header("Ranges")] 
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float attackRange = 2f;

        [Header("Patrol Points")] 
        public Transform[] patrolPoints;

        [Header("Rewards")] 
        public float xp = 50f;
        public float xpModifier = 1f;
        
        private float _curHp;

        private EnemyState _currentState;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Damage = Animator.StringToHash("Damage");

        protected void Awake()
        {
            _curHp = maxHp;
            _currentState = new EnemyIdleState(this);
        }

        protected virtual void Start()
        {
            _currentState.EnterState();
        }
        
        protected virtual void Update()
        {
            _currentState.UpdateState();
        }

        protected virtual void FixedUpdate()
        {
            _currentState.FixedUpdateState();
        }

        public void TransitionToState(EnemyState newState)
        {
            _currentState.ExitState();
            _currentState = newState;
            _currentState.EnterState();
        }
        
        // Set the speed variable in animator for blent tree
        public virtual void SetMovementSpeed(float speed)
        {
            if (!animator) return;
            
            animator.SetFloat(Speed, speed);
        }
        
        // Return true if the player is close
        public bool IsPlayerDetected()
        {
            if (!playerTransform) return false;
            
            return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
        }
        
        // Return true if the player is in attack range
        public bool IsPlayerInAttackRange()
        {
            if (!playerTransform) return false;
            
            return Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
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

        public virtual void TakeDamage(float damageAmount)
        {
            _curHp -= damageAmount;
            
            healthBar.UpdateHealthBar(_curHp, maxHp);

            if (_curHp <= 0)
            {
                _curHp = 0;
                OnEnemyDeath?.Invoke(xp * xpModifier);
                TransitionToState(new EnemyDeathState(this));
                Destroy(gameObject, 3f);
                return;
            }
            
            animator.SetTrigger(Damage);
        }
    }
}