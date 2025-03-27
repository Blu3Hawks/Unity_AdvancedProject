using System.Collections;
using UnityEngine;

namespace PlayerStates
{
    public class DefendState : PlayerState
    {
        private float _parryTimer;
        
        private AnimationClip _blockAnimation;
        private float _blockDuration;
        
        private static readonly int Block = Animator.StringToHash("Block");

        public DefendState(PlayerController player, Transform camera) : base(player, camera)
        {
            _blockAnimation = Player.blockAnimation;
        }

        public override void EnterState()
        {
            _blockDuration = _blockAnimation.length / 1.8f;
            
            _parryTimer = Player.parryWindow;
            
            Player.animator.SetTrigger(Block);
            
            Player.IsParrying = true;
            Player.IsBlocking = true;

            Player.StartCoroutine(EndOfBlock());
        }

        public override void ExitState()
        {
            Player.animator.ResetTrigger(Block);
            Player.IsBlocking = false;
        }

        public override void HandleInput()
        {
            
        }

        public override void UpdateState()
        {
            _parryTimer -= Time.deltaTime;

            if (_parryTimer <= 0)
                Player.IsParrying = false;
        }

        public override void FixedUpdateState()
        {
            
        }

        private IEnumerator EndOfBlock()
        {
            yield return new WaitForSeconds(_blockDuration);
            
            Player.TransitionToState(new IdleState(Player, Player.cameraTransform));
        }
    }
}