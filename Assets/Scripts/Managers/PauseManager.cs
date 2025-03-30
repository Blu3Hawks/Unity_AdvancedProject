using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        public event UnityAction OnPauseEnter;
        public event UnityAction OnPauseClose;
        
        [Header("References")]
        [SerializeField] private PlayerInput playerInput;
        
        private const string PlayerActionMap = "Player";
        private const string UiActionMap = "UI";

        private bool _isPause;

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            _isPause = !_isPause;

            if (_isPause)
            {
                // Pause game
                Time.timeScale = 0f;
                
                // Switch to UI input
                playerInput.SwitchCurrentActionMap(UiActionMap);
                
                // Invoke pause enter event
                OnPauseEnter?.Invoke();
            }
            else
            {
                // Resume Play
                Time.timeScale = 1f;
                
                // Switch back to gameplay input
                playerInput.SwitchCurrentActionMap(PlayerActionMap);
                
                // Invoke pause out event
                OnPauseClose?.Invoke();
            }
        }

        public void SetupPlayerScripts(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }
    }
}
