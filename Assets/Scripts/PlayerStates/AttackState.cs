using System.Collections;
using UnityEngine;

namespace PlayerStates
{
    public class AttackState : PlayerState
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private AnimationClip _attackAnimation;
        private float _attackDuration;
        private bool _attackComplete;

        public AttackState(PlayerController player, Transform camera) : base(player, camera)
        {
            _attackAnimation = Player.attackAnimation;
        }

        public override void EnterState()
        {
            Player.animator.SetTrigger(Attack);
            _attackDuration = _attackAnimation.length;
            Player.weapon.EnableCollider();
            Player.StartCoroutine(EndOfAttack());
        }

        public override void ExitState()
        {
            Player.animator.ResetTrigger(Attack);
            Player.weapon.DisableCollider();
        }

        public override void HandleInput()
        {
            Input = Player.GetMoveInput();
        }

        public override void UpdateState()
        {
            
        }

        public override void FixedUpdateState()
        {
            
        }

        private IEnumerator EndOfAttack()
        {
            yield return new WaitForSeconds(_attackDuration);
            
            Player.TransitionToState(new IdleState(Player, Player.cameraTransform));
        }
    }
}