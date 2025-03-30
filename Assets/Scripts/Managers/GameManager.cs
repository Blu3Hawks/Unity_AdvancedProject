using Enemies;
using System.Collections;
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
        [SerializeField] private GameplayAudioManager audioManager;


        private void OnDisable()
        {
            player.OnHit -= uiManager.UpdatePlayerHpBar;
            player.OnParry -= uiManager.SetDamageMultiplierTxt;
            player.OnParry -= audioManager.PlayParrySfx;
        }

        public void SetupEvents()
        {
            foreach (Enemy enemy in enemySpawner.ListOfEnemies)
            {
                enemy.OnEnemyDeath += levelUpSystem.AddXp;
                enemy.OnEnemyDeath += uiManager.UpdateXpBar;
            }
            StartCoroutine(AddingPlayerSFX());
        }

        private IEnumerator AddingPlayerSFX()
        {
            yield return new WaitForSeconds(0.04f);
            player.OnParry += audioManager.PlayParrySfx;

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
            Debug.Log(player, uiManager);

        }
    }
}
