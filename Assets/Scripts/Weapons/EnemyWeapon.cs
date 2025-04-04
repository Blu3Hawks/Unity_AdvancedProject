using UnityEngine;

namespace Weapons
{
    public class EnemyWeapon : Weapon
    {
        private bool _isPlayerHit;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var player = other.GetComponent<PlayerController>();
            
            // If the player is valid and wasn't hit in the same attack animation - hit him
            if (player && !_isPlayerHit)
            {
                _isPlayerHit = true;
                player.TakeDamage(damage);
            }
        }

        public override void ResetPlayerHit()
        {
            _isPlayerHit = false;
        }
    }
}