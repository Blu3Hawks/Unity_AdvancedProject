using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class PlayerWeapon : Weapons.Weapon
    {
        [Header("References")] 
        [SerializeField] private PlayerController player;
        
        private HashSet<Enemies.Enemy> _hitEnemies = new ();
        
        public override void EnableCollider()
        {
            _hitEnemies.Clear();
            
            base.EnableCollider();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;

            var enemy = other.GetComponent<Enemies.Enemy>();

            if (enemy)
            {
                if (!_hitEnemies.Contains(enemy))
                {
                    enemy.TakeDamage(damage);
                    _hitEnemies.Add(enemy);
                }
            }
        }
    }
}