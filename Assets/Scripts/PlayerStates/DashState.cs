using UnityEngine;

namespace PlayerStates
{
    public class DashState : PlayerState
    {
        private float _dashTimeRemaining;
        
        public DashState(PlayerController player) : base(player)
        {
        }

        public override void EnterState()
        {
            _dashTimeRemaining = Player.dashDuration;
            
            // Get the dash direction from input.
            var input = Player.GetMoveInput();
            
            if (input == Vector2.zero)
            {
                // If no input, dash in player's forward direction.
                input = new Vector2(Player.transform.forward.x, Player.transform.forward.z);
            }
            
            // Convert input to a world space dash direction relative to the player.
            var dashMovement = new Vector3(input.x, 0f, input.y) * Player.dashSpeed;
            Player.rb.linearVelocity = new Vector3(dashMovement.x, Player.rb.linearVelocity.y, dashMovement.z);
        }

        public override void ExitState()
        {
            
        }

        public override void HandleInput()
        {
            
        }

        public override void UpdateState()
        {
            _dashTimeRemaining -= Time.deltaTime;
            
            if (_dashTimeRemaining <= 0)
            {
                Player.TransitionToState(new MoveState(Player));
            }
        }

        public override void FixedUpdateState()
        {
            
        }
    }
}