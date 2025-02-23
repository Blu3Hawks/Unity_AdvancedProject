using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public event UnityAction<int> OnCoinPickUp;

        private int _score;
        
        [SerializeField] private PlayerController player;
        [SerializeField] private LevelManager levelManager;
        
        private void Start()
        {
            foreach (var coin in levelManager.coins)
            {
                coin.OnCollected += CoinOnCollected;
            }
        }

        private void CoinOnCollected(Coin coin)
        {
            var coinScore = coin.GetScore();
            _score += coinScore;
            OnCoinPickUp?.Invoke(_score);
        }
    }
}
