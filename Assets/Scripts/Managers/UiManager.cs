using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SettingManager settingManager;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private LevelUpSystem levelUpSystem;

        [Header("References")] 
        
        [Header("Sound UI")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        
        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject deathMenu;
        
        [Header("Character HUD")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI damageMultiplier;
        
        [Header("Xp Bar")]
        [SerializeField] private XpBar xpBar;
        [SerializeField] private TextMeshProUGUI curXpTxt;
        [SerializeField] private TextMeshProUGUI xpToNextLevelTxt;


        private void OnDisable()
        {
            pauseManager.OnPauseEnter -= OpenPauseMenu;
            pauseManager.OnPauseClose -= ClosePauseMenu;
            levelUpSystem.OnLevelUp -= SetLevelText;
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

        public void UpdatePlayerHpBar(float curHp, float maxHp)
        {
            healthBar.UpdateHealthBar(curHp, maxHp);
        }

        public void UpdateXpBar(float xp)
        {
            xpBar.UpdateXpBar(levelUpSystem.CurXp, levelUpSystem.XpToNextLevel);
            curXpTxt.text = levelUpSystem.CurXp.ToString("F0");
            xpToNextLevelTxt.text = levelUpSystem.XpToNextLevel.ToString("F0");
        }

        public void SetLevelText(int level)
        {
            levelText.text = level.ToString("D2");
        }

        public void SetDamageMultiplierTxt(float multiplier)
        {
            damageMultiplier.text = multiplier.ToString("F2");
        }

        public void ResetDamageMultiplier(float curHp, float maxHp)
        {
            damageMultiplier.text = 1f.ToString("F2");
        }

        public void SetupPlayerScripts(LevelUpSystem levelUpSystem)
        {
            this.levelUpSystem = levelUpSystem;
        }

        public void AddSubscribersToPlayerDeath(PlayerController player)
        {
            player.OnPlayerDeath += DeathMethods;
        }

        private void DeathMethods()
        {
            Time.timeScale = 0f;
            deathMenu.SetActive(true);
        }

        public void OnReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("Main Menu");
            DataPersistenceManager.Instance.DeleteProfileId(DataPersistenceManager.Instance.SelectedProfileId);
        }

        public void OnExitGame()
        {
            Time.timeScale = 1f;
            Application.Quit();
            DataPersistenceManager.Instance.DeleteProfileId(DataPersistenceManager.Instance.SelectedProfileId);
        }

    }
}
