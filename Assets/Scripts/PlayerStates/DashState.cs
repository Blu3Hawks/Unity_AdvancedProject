using UnityEngine;

namespace PlayerStates
{
    public class DashState : PlayerState
    {
        private float _dashTimeRemaining;
        
        public DashState(PlayerController player, Transform camera, Vector2 input) : base(player, camera, input)
        {
        }

        public override void EnterState()
        {
            _dashTimeRemaining = Player.dashDuration;
            
            // Get the dash direction from input.
            Input = Player.GetMoveInput();
            
            if (Input == Vector2.zero)
            {
                // If no input, dash in player's forward direction.
                Input = new Vector2(Player.transform.forward.x, Player.transform.forward.z);
            }
            
            // Convert input to a world space dash direction relative to the player.
            var dashMovement = new Vector3(Input.x, 0f, Input.y) * Player.dashSpeed;
            Player.rb.linearVelocity = new Vector3(dashMovement.x, Player.rb.linearVelocity.y, dashMovement.z);
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
            _dashTimeRemaining -= Time.deltaTime;
            
            if (_dashTimeRemaining <= 0)
            {
                Player.TransitionToState(new MoveState(Player, Player.cameraTransform, Input));
            }
        }

        public override void FixedUpdateState()
        {
            
        }
    }
}