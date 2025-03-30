using Chen_s_Folder.Scripts.Enemies;
using Enemies;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController player;
        [SerializeField] private RandomEnemySpawner enemySpawner;
        [SerializeField] private LevelUpSystem levelUpSystem;
        [SerializeField] private UiManager uiManager;

        private void OnDisable()
        {
            player.OnHit -= uiManager.UpdatePlayerHpBar;
            player.OnParry -= uiManager.SetDamageMultiplierTxt;
        }

        public void SetupEvents()
        {
            foreach (Enemy enemy in enemySpawner.ListOfEnemies)
            {
                enemy.OnEnemyDeath += levelUpSystem.AddXp;
                enemy.OnEnemyDeath += uiManager.UpdateXpBar;
            }
        }

        public void RemoveEnemiesEvents()
        {
            foreach (Enemy enemy in enemySpawner.ListOfEnemies)
            {
                enemy.OnEnemyDeath -= levelUpSystem.AddXp;
                enemy.OnEnemyDeath -= uiManager.UpdateXpBar;
            }
        }

        public void SetupGameManagerEventsAndScripts(PlayerController playerController, LevelUpSystem levelUpSystem)
        {
            player = playerController;
            this.levelUpSystem = levelUpSystem;
        }
    }
}
