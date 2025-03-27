using UnityEngine;

namespace PlayerStates
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player, Transform camera) : base(player, camera)
        {
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void HandleInput()
        {
            Input = Player.GetMoveInput();
        }

        public override void UpdateState()
        {
            if(Player.MovePressed())
                Player.TransitionToState(new MoveState(Player, Player.cameraTransform));
            
            else if (Player.JumpPressed() && Player.IsGrounded())
                Player.TransitionToState(new JumpState(Player, Player.cameraTransform));
            
            else if (Player.DashPressed())
                Player.TransitionToState(new DashState(Player, Player.cameraTransform));
            
            else if (Player.AttackPressed())
                Player.TransitionToState(new AttackState(Player, Player.cameraTransform));
            
            else if (Player.BlockPressed())
                Player.TransitionToState(new DefendState(Player, Player.cameraTransform));
        }

        public override void FixedUpdateState()
        {
            
        }
    }
}