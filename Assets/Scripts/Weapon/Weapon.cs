using UnityEngine;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected float damage = 10f;
        [SerializeField] private Collider weaponCollider;

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
    }
}
