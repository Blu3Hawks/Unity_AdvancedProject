using UnityEngine;

namespace PlayerStates
{
    public abstract class PlayerState
    {
        protected PlayerController Player;
        protected Transform Camera;
        protected Vector2 Input;

        public PlayerState(PlayerController player, Transform camera)
        {
            Player = player;
            Camera = camera;
        }
        
        // Called when entering the state
        public abstract void EnterState();

        // Called when leaving the state
        public abstract void ExitState();

        // Handle input for this state
        public abstract void HandleInput();

        // Update logic for the state
        public abstract void UpdateState();
        
        // Fixed Update logic for state
        public abstract void FixedUpdateState();
    }
}