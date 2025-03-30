using UnityEngine;

namespace Enemies.States
{
    public class EnemyPatrolState : EnemyState
    {
        private int _currentIndex;
        private Vector3[] _patrolPoints;

        public EnemyPatrolState(Enemy enemy) : base(enemy)
        {
            _currentIndex = 0;
            _patrolPoints = new Vector3[2];
        }

        public override void EnterState()
        {
            if (!Enemy.animator) return;

            //Enemy.animator.SetBool(IsMoving, true);
            Enemy.SetMovementSpeed(Enemy.patrolSpeed);
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
            if (_patrolPoints == null || _patrolPoints.Length < 2) return;

            // Calculate the direction to the target point
            var target = _patrolPoints[_currentIndex];
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
                _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
            }
        }

        public override void ExitState()
        {
            if (!Enemy.animator) return;

            //Enemy.animator.SetBool(IsMoving, false);
        }

        public void SetPatrolPoints(Vector3 point1, Vector3 point2)
        {
            _patrolPoints[0] = point1;
            _patrolPoints[1] = point2;
        }
    }
}