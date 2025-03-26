using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class Enemy : IDamageable
    {
        private float _maxHp;
        private float _curHp;

        public Enemy(float maxHp)
        {
            _maxHp = maxHp;
            _curHp = _maxHp;
        }
        
        public void TakeDamage(float damage)
        {
            _curHp -= damage;

            if (_curHp <= 0)
            {
                _curHp = 0;
                // TODO: Create Death Event
            }
            
            Debug.Log(_curHp);
        }
    }
}