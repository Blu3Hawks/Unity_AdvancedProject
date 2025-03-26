using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private Collider weaponCollider;

    private HashSet<EnemyController> _hitEnemies = new ();

    private void Start()
    {
        weaponCollider.enabled = false;
    }

    public void EnableCollider()
    {
        _hitEnemies.Clear();
        weaponCollider.enabled = true;
    }

    public void DisableCollider()
    {
        weaponCollider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        var enemy = other.GetComponent<EnemyController>();

        if (enemy)
        {
            if (!_hitEnemies.Contains(enemy))
            {
                enemy.enemy.TakeDamage(damage);
                _hitEnemies.Add(enemy);
                Debug.Log("Enemy Hit!");
            }
        }
    }
}
