using System;

namespace PlayerStates
{
    public class AttackState : PlayerState
    {
        public AttackState(PlayerController player) : base(player)
        {
        }

        public override void EnterState()
        {
            Player.Attack();
        }

        public override void ExitState()
        {
            
        }

        public override void HandleInput()
        {
            if (Player.AttackComplete())
            {
                Player.TransitionToState(new MoveState(Player));
            }
        }

        public override void UpdateState()
        {
            
        }

        public override void FixedUpdateState()
        {
            
        }
    }
}