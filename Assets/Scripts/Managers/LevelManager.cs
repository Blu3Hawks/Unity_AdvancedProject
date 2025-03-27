using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] public List<Coin> coins;
        public List<Enemy.Enemy> enemies;

        public void RemoveCoin(Coin coin)
        {
            coins.Remove(coin);
        }
    }
}
