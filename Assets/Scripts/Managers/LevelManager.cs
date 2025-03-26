using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private int currentLevel;
        public int CurrentLevel { get { return currentLevel; } }

        [SerializeField] public List<Coin> coins;

        public void RemoveCoin(Coin coin)
        {
            coins.Remove(coin);
        }
    }
}
