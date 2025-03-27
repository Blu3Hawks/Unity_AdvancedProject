using UnityEngine;

namespace Enemy.States
{
    public class EnemyAttackState : EnemyState
    {
        private static readonly int Attack = Animator.StringToHash("RightAttack");
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private float _attackTimer;

        public EnemyAttackState(Enemy enemy) : base(enemy)
        {
        }

        public override void EnterState()
        {
            Enemy.animator.SetFloat(Speed, 0f);

            _attackTimer = Enemy.attackDuration;
            
            Enemy.weapon.EnableCollider();
            
            if(!Enemy.animator) return;
            Enemy.animator.SetTrigger(Attack);
        }

        public override void UpdateState()
        {
            _attackTimer -= Time.deltaTime;

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
            
            if(!Enemy.animator) return;
            Enemy.animator.ResetTrigger(Attack);
        }
    }
}