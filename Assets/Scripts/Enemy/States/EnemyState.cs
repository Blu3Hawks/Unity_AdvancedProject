namespace Enemy.States
{
    public abstract class EnemyState
    {
        protected Enemy Enemy;

        public EnemyState(Enemy enemy)
        {
            Enemy = enemy;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void FixedUpdateState();
        public abstract void ExitState();
    }
}