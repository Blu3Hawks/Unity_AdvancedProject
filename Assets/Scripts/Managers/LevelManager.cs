using System.Collections.Generic;
using UnityEngine;

// UNUSED SCRIPT

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private int _currentLevel;
        public int CurrentLevel { get { return _currentLevel; } }

        [SerializeField] public List<Coin> coins;

        public void RemoveCoin(Coin coin)
        {
            coins.Remove(coin);
        }
    }
}
