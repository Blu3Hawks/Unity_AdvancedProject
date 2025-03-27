using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        public float damage;
        [SerializeField] private float baseDamage = 10f;
        [SerializeField] private Collider weaponCollider;

        private void Awake()
        {
            damage = baseDamage;
        }

        private void Start()
        {
            weaponCollider.enabled = false;
        }

        public virtual void EnableCollider()
        {
            weaponCollider.enabled = true;
        }

        public virtual void DisableCollider()
        {
            weaponCollider.enabled = false;
        }

        public virtual void SetDamageWithMultiplier(float multiplier)
        {
            damage *= multiplier;
        }
    }
}
