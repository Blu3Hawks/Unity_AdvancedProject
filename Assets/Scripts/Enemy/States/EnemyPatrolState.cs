using UnityEngine;

namespace Enemy.States
{
    public class EnemyPatrolState : EnemyState
    {
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        private int _currentIndex;

        public EnemyPatrolState(Enemy enemy) : base(enemy)
        {
            _currentIndex = 0;
        }

        public override void EnterState()
        {
            if (!Enemy.animator) return;
            
            Enemy.animator.SetBool(IsMoving, true);
        }

        public override void UpdateState()
        {
            if (Enemy.IsPlayerDetected())
            {
                Enemy.TransitionToState(new EnemyChaseState(Enemy));
            }
        }

        public override void FixedUpdateState()
        {
            if (Enemy.patrolPoints == null || Enemy.patrolPoints.Length == 0) return;
            
            // Calculate the direction to the target point
            var target = Enemy.patrolPoints[_currentIndex].position;
            var direction = (target - Enemy.transform.position).normalized;
            
            // Move toward the target point
            Enemy.transform.position += direction * (Enemy.patrolSpeed * Time.deltaTime);

            // Rotate toward the patrol point.
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, targetRotation, 5f * Time.deltaTime);
            }

            // If close enough to the target point, move to the next patrol point.
            if (Vector3.Distance(Enemy.transform.position, target) < 0.5f)
            {
                _currentIndex = (_currentIndex + 1) % Enemy.patrolPoints.Length;
            }
        }

        public override void ExitState()
        {
            if (!Enemy.animator) return;
            
            Enemy.animator.SetBool(IsMoving, false);
        }
    }
}