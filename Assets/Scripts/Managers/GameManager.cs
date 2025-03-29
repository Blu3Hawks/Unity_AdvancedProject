using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController player;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private LevelUpSystem levelUpSystem;
        [SerializeField] private UiManager uiManager;

        private void OnEnable()
        {
            foreach (var enemy in levelManager.enemies)
            {
                enemy.OnEnemyDeath += levelUpSystem.AddXp;
                enemy.OnEnemyDeath += uiManager.UpdateXpBar;
            }

            player.OnHit += uiManager.UpdatePlayerHpBar;
            player.OnParry += uiManager.SetDamageMultiplierTxt;
        }

        private void OnDisable()
        {
            foreach (var enemy in levelManager.enemies)
            {
                enemy.OnEnemyDeath -= levelUpSystem.AddXp;
                enemy.OnEnemyDeath -= uiManager.UpdateXpBar;
            }

            player.OnHit -= uiManager.UpdatePlayerHpBar;
            player.OnParry -= uiManager.SetDamageMultiplierTxt;
        }
    }
}
