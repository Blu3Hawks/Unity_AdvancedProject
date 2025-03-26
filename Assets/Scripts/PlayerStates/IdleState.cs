using UnityEngine;

namespace PlayerStates
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player, Transform camera, Vector2 input) : base(player, camera, input)
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
                Player.TransitionToState(new MoveState(Player, Player.cameraTransform, Input));
            
            else if (Player.JumpPressed() && Player.IsGrounded())
                Player.TransitionToState(new JumpState(Player, Player.cameraTransform, Input));
            
            else if (Player.DashPressed())
                Player.TransitionToState(new DashState(Player, Player.cameraTransform, Input));
            
            else if (Player.AttackPressed())
                Player.TransitionToState(new AttackState(Player, Player.cameraTransform, Input));
        }

        public override void FixedUpdateState()
        {
            
        }
    }
}