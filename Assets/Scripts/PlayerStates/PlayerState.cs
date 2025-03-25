
namespace PlayerStates
{
    public abstract class PlayerState
    {
        protected PlayerController Player;

        public PlayerState(PlayerController player)
        {
            Player = player;
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