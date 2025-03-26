using UnityEngine;

namespace Enemy.States
{
    public class EnemyChaseState : EnemyState
    {
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        public EnemyChaseState(Enemy enemy) : base(enemy)
        {
        }

        public override void EnterState()
        {
            if (!Enemy.animator) return;
            
            Enemy.animator.SetBool(IsRunning, true);
        }

        public override void UpdateState()
        {
            if (Enemy.IsPlayerInAttackRange())
                Enemy.TransitionToState(new EnemyAttackState(Enemy));
            
            else if (!Enemy.IsPlayerDetected())
                Enemy.TransitionToState(new EnemyPatrolState(Enemy));
            
        }

        public override void FixedUpdateState()
        {
            if (!Enemy.playerTransform) return;

            // Move toward the player.
            var direction = (Enemy.playerTransform.position - Enemy.transform.position).normalized;
            Enemy.transform.position += direction * (Enemy.chaseSpeed * Time.deltaTime);

            // Rotate toward the player.
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }

        public override void ExitState()
        {
            if (!Enemy.animator) return;
            
            Enemy.animator.SetBool(IsRunning, false);
        }
    }
}