using UnityEngine;

namespace PlayerStates
{
    public class JumpState : PlayerState
    {
        private float _jumpTimer;
        private const float JumpOffSet = 0.2f;

        private Vector2 _input;

        public JumpState(PlayerController player) : base(player)
        {
        }

        public override void EnterState()
        {
            Player.rb.linearVelocity = new Vector3(Player.rb.linearVelocity.x, Player.jumpForce, Player.rb.linearVelocity.z);
            _jumpTimer = JumpOffSet;
        }

        public override void ExitState()
        {
            
        }

        public override void HandleInput()
        {
            _input = Player.GetMoveInput();
        }

        public override void UpdateState()
        {
            _jumpTimer -= Time.deltaTime;
            if (_jumpTimer >= 0)
                return;
            
            if (Player.IsGrounded())
            {
                Player.TransitionToState(new MoveState(Player));
            }
        }

        public override void FixedUpdateState()
        {
            var horizontalMovement = new Vector3(_input.x, 0f, _input.y) * Player.moveSpeed;
            
            Player.rb.linearVelocity = new Vector3(horizontalMovement.x, Player.rb.linearVelocity.y, horizontalMovement.z);
        }
    }
}