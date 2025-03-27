using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SettingManager settingManager;
        [SerializeField] private PauseManager pauseManager;

        [Header("References")] 
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private void Awake()
        {
            pauseManager.OnPauseEnter += OpenPauseMenu;
            pauseManager.OnPauseClose += ClosePauseMenu;
        }

        private void SetScoreText(int score)
        {
            scoreText.text = score.ToString("D6");
        }

        public void SetMusicSlider(float value)
        {
            musicSlider.value = value;
        }

        public void SetSfxSlider(float value)
        {
            sfxSlider.value = value;
        }

        private void OpenPauseMenu()
        {
            // Open pause menu
            pauseMenu.SetActive(true);
        }

        private void ClosePauseMenu()
        {
            // Close pause menu
            pauseMenu.SetActive(false);
        }
    }
}
