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
        [SerializeField] private PauseManager pauseManager;

        private const string PlayerActionMap = "Player";
        private const string UiActionMap = "UI";

        private bool _isPause = false;

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {

            if (!_isPause)
            {
                _isPause = true;
                // Pause game
                Time.timeScale = 0f;

                // Switch to UI input
                playerInput.SwitchCurrentActionMap(UiActionMap);

                // Invoke pause enter event
                OnPauseEnter?.Invoke();



            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log(_isPause);
        }

        public void OnUnpause()
        {
            if (_isPause)
            {

                _isPause = false;
                // Resume Play
                Time.timeScale = 1f;

                // Switch back to gameplay input
                playerInput.SwitchCurrentActionMap(PlayerActionMap);

                // Invoke pause out event
                OnPauseClose?.Invoke();

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            Debug.Log(_isPause);

        }

        public void SetupPlayerScripts(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
            var uiActionMap = this.playerInput.actions.FindActionMap(PlayerActionMap);
            if (uiActionMap != null)
            {
                var pauseAction = uiActionMap.FindAction("Pause");
                if (pauseAction != null)
                {
                    pauseAction.performed -= OnPause; // Unsubscribe first
                    pauseAction.performed += OnPause; // Then subscribe
                }
            }
        }
    }
}
