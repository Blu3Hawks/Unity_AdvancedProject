using System;
using UnityEngine;

namespace Weapon
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
    }
}
