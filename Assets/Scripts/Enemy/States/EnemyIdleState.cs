namespace Enemy.States
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(Enemy enemy) : base(enemy)
        {
        }

        public override void EnterState()
        {
            
        }

        public override void UpdateState()
        {
            if (Enemy.IsPlayerDetected())
            {
                Enemy.TransitionToState(new EnemyChaseState(Enemy));
            }
            else
            {
                Enemy.TransitionToState(new EnemyPatrolState(Enemy));
            }
        }

        public override void FixedUpdateState()
        {
            
        }

        public override void ExitState()
        {
            
        }
    }
}