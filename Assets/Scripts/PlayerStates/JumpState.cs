using UnityEngine;

namespace PlayerStates
{
    public class JumpState : PlayerState
    {
        private float _jumpTimer;
        private const float JumpOffSet = 0.2f;

        private static readonly int Jump = Animator.StringToHash("Jump");

        public JumpState(PlayerController player, Transform camera, Vector2 input) : base(player, camera, input)
        {
        }

        public override void EnterState()
        {
            Player.animator.SetTrigger(Jump);
            _jumpTimer = JumpOffSet;
        }

        public override void ExitState()
        {
            Player.animator.ResetTrigger(Jump);
        }

        public override void HandleInput()
        {
            Input = Player.GetMoveInput();
        }

        public override void UpdateState()
        {
            // This is off set timer so the jump state won't gat out before the player even left the ground
            _jumpTimer -= Time.deltaTime;
            if (_jumpTimer >= 0)
                return;
            
            if (Player.IsGrounded())
            {
                Player.TransitionToState(new IdleState(Player, Player.cameraTransform, Input));
            }
        }

        public override void FixedUpdateState()
        {
            // Get the camera forward direction
            var camForward = Camera.forward;
            camForward.y = 0f;
            camForward.Normalize();
            
            // Get The camera right direction
            var camRight = Camera.right;
            camRight.y = 0;
            camRight.Normalize();
            
            // Combine it with the input
            var moveDir = (camForward * Input.y + camRight * Input.x).normalized;
            
            // Set the direction of the movement
            Player.rb.linearVelocity = moveDir * Player.moveSpeed + new Vector3(0f, Player.rb.linearVelocity.y, 0f);
            
            // Rotate the player the way the camera is facing
            var targetRotation = Quaternion.LookRotation(moveDir);
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotation, Player.speedRotation * Time.deltaTime);
        }
    }
}