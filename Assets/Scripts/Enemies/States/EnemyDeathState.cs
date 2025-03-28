using UnityEngine;

namespace Enemies.States
{
    public class EnemyDeathState : EnemyState
    {
        private static readonly int Death = Animator.StringToHash("Death");
        
        public EnemyDeathState(Enemy enemy) : base(enemy)
        {
        }

        public override void EnterState()
        {
            Enemy.animator.SetTrigger(Death);
        }

        public override void UpdateState()
        {
            
        }

        public override void FixedUpdateState()
        {
            
        }

        public override void ExitState()
        {
            
        }
    }
}