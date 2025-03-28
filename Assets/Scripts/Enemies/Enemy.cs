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

        protected EnemyState CurrentState;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Damage = Animator.StringToHash("Damage");
        private static readonly int Death = Animator.StringToHash("Death");

        protected void Awake()
        {
            _curHp = maxHp;
            CurrentState = new EnemyIdleState(this);
        }

        protected virtual void Start()
        {
            CurrentState.EnterState();
        }
        
        protected virtual void Update()
        {
            CurrentState.UpdateState();
        }

        protected virtual void FixedUpdate()
        {
            CurrentState.FixedUpdateState();
        }

        public void TransitionToState(EnemyState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }

        public virtual void SetMovementSpeed(float speed)
        {
            if (!animator) return;
            
            animator.SetFloat(Speed, speed);
        }

        public bool IsPlayerDetected()
        {
            if (!playerTransform) return false;
            
            return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
        }

        public bool IsPlayerInAttackRange()
        {
            if (!playerTransform) return false;
            
            return Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
        }

        public void EnableWeaponCollider()
        {
            weapon.EnableCollider();
        }

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
                animator.SetTrigger(Death);
                Destroy(gameObject, 0.5f);
                return;
            }
            
            animator.SetTrigger(Damage);
            Debug.Log(_curHp);
        }
    }
}