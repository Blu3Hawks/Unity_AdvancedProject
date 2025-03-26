using UnityEngine;

namespace PlayerStates
{
    public class AttackState : PlayerState
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private AnimationClip _attackAnimation;
        private float _attackDuration;
        private bool _attackComplete;

        public AttackState(PlayerController player, Transform camera, Vector2 input) : base(player, camera, input)
        {
            _attackAnimation = Player.attackAnimation;
        }

        public override void EnterState()
        {
            Player.animator.SetTrigger(Attack);
            _attackDuration = _attackAnimation.length;
        }

        public override void ExitState()
        {
            Player.animator.ResetTrigger(Attack);
        }

        public override void HandleInput()
        {
            Input = Player.GetMoveInput();
        }

        public override void UpdateState()
        {
            _attackDuration -= Time.deltaTime;

            if (_attackDuration <= 0)
                _attackComplete = true;
            
            if(_attackComplete)
                Player.TransitionToState(new IdleState(Player, Player.cameraTransform, Input));
        }

        public override void FixedUpdateState()
        {
            
        }
    }
}