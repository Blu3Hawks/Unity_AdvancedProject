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

        private void Awake()
        {
            foreach (var enemy in levelManager.enemies)
            {
                enemy.OnEnemyDeath += levelUpSystem.AddXp;
            }

            player.OnHit += uiManager.UpdatePlayerHpBar;
        }
    }
}
