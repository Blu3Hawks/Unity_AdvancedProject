using UnityEngine;

namespace PlayerStates
{
    public class MoveState : PlayerState
    {
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        public MoveState(PlayerController player, Transform camera, Vector2 input) : base(player, camera, input)
        {
        }

        public override void EnterState()
        {
            Player.animator.SetBool(IsMoving, true);
        }

        public override void ExitState()
        {
            Player.animator.SetBool(IsMoving, false);
        }

        public override void HandleInput()
        {
            Input = Player.GetMoveInput();
        }

        public override void UpdateState()
        {
            if (Input == Vector2.zero)
                Player.TransitionToState(new IdleState(Player, Player.cameraTransform, Input));
            
            else if (Player.JumpPressed() && Player.IsGrounded())
                Player.TransitionToState(new JumpState(Player, Player.cameraTransform, Input));
            
            else if (Player.DashPressed())
                Player.TransitionToState(new DashState(Player, Player.cameraTransform, Input));
            
            else if (Player.AttackPressed())
                Player.TransitionToState(new AttackState(Player, Player.cameraTransform, Input));
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