using System.Collections.Generic;
using UnityEngine;
using Enemies;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private int currentLevel;
        public int CurrentLevel { get { return currentLevel; } }

        [SerializeField] public List<Coin> coins;
        //public List<Enemy> listOfEnemies;

        public void RemoveCoin(Coin coin)
        {
            coins.Remove(coin);
        }
    }
}
