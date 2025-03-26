using UnityEngine;

namespace Enemy.States
{
    public class EnemyAttackState : EnemyState
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        private float _attackDuration = 2f;
        private float _attackTimer;

        public EnemyAttackState(Enemy enemy) : base(enemy)
        {
        }

        public override void EnterState()
        {
            _attackTimer = _attackDuration;
            
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
            
        }

        public override void ExitState()
        {
            if(!Enemy.animator) return;
            
            Enemy.animator.ResetTrigger(Attack);
        }
    }
}