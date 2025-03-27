using Enemy.States;
using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour , IDamageable
    {
        [Header("References")] 
        public Animator animator;
        public Transform playerTransform;
        public Weapon.Weapon weapon;
        
        [Header("Stats")]
        [SerializeField] private float _maxHp;
        public float patrolSpeed = 1f;
        public float chaseSpeed = 2f;
        public float attackDuration = 3f;

        [Header("Ranges")] 
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float attackRange = 2f;

        [Header("Patrol Points")] 
        public Transform[] patrolPoints;
        
        private float _curHp;

        protected EnemyState CurrentState;
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Damage = Animator.StringToHash("Damage");

        protected void Awake()
        {
            _curHp = _maxHp;
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

        public virtual void TakeDamage(float damage)
        {
            _curHp -= damage;

            if (_curHp <= 0)
            {
                _curHp = 0;
                // TODO: Create Death Event
            }
            
            animator.SetTrigger(Damage);
            Debug.Log(_curHp);
        }
    }
}