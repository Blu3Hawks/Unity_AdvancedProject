using UnityEngine;

namespace Enemies.States
{
    public class EnemyAttackState : EnemyState
    {
        private static readonly int Attack = Animator.StringToHash("RightAttack");
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private float _attackTimer;
        
        private float _attackOffSetDuration = 0.15f;
        private float _startAttack;
        private bool _isColliderEnabled;

        public EnemyAttackState(Enemy enemy) : base(enemy)
        {
        }

        public override void EnterState()
        {
            Enemy.animator.SetFloat(Speed, 0f);

            _attackTimer = Enemy.attackDuration;

            _startAttack = _attackOffSetDuration;
            
            if(!Enemy.animator) return;
            Enemy.animator.SetTrigger(Attack);
        }

        public override void UpdateState()
        {
            _attackTimer -= Time.deltaTime;
            _startAttack -= Time.deltaTime;

            if (_startAttack <= 0 && !_isColliderEnabled)
            {
                Enemy.weapon.EnableCollider();
                _isColliderEnabled = true;
            }

            if (!(_attackTimer <= 0)) return;

            if (Enemy.IsPlayerInAttackRange())
                Enemy.TransitionToState(new EnemyAttackState(Enemy));
            
            else if (Enemy.IsPlayerDetected())
                Enemy.TransitionToState(new EnemyChaseState(Enemy));
            
            else
                Enemy.TransitionToState(new EnemyIdleState(Enemy));
        }

        public override void FixedUpdateState()
        {
            // Move toward the player.
            var direction = (Enemy.playerTransform.position - Enemy.transform.position).normalized;

            // Rotate toward the player.
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }

        public override void ExitState()
        {
            Enemy.weapon.DisableCollider();
            Enemy.weapon.ResetPlayerHit();
            
            if(!Enemy.animator) return;
            Enemy.animator.ResetTrigger(Attack);
        }
    }
}