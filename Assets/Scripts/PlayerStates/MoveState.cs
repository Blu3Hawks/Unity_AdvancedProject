using UnityEngine;

namespace PlayerStates
{
    public class MoveState : PlayerState
    {
        private Vector2 _input;
        
        public MoveState(PlayerController player) : base(player)
        {
        }

        public override void EnterState()
        {
            // TODO: Set animations flag
        }

        public override void ExitState()
        {
            
        }

        public override void HandleInput()
        {
            _input = Player.GetMoveInput();
            
            if (Player.JumpPressed() && Player.IsGrounded())
                Player.TransitionToState(new JumpState(Player));
            
            else if (Player.DashPressed())
                Player.TransitionToState(new DashState(Player));
            
            else if (Player.AttackPressed())
                Player.TransitionToState(new AttackState(Player));
        }

        public override void UpdateState()
        {
            
        }

        public override void FixedUpdateState()
        {
            var horizontalMovement = new Vector3(_input.x, 0f, _input.y) * Player.moveSpeed;
            
            Player.rb.linearVelocity = new Vector3(horizontalMovement.x, Player.rb.linearVelocity.y, horizontalMovement.z);
        }
    }
}