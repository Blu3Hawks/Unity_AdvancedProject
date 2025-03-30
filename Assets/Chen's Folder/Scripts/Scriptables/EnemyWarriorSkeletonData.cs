using Enemies;
using UnityEngine;

namespace Chen_s_Folder.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Enemy Skeleton Warrior Data", menuName = "Scriptable Objects/Enemy Skeleton Warrior Data", order = 3)]
    public class EnemyWarriorSkeletonData : ScriptableObject
    {
        public GameObject prefab;
        public WarriorSkeletonEnemy enemyScript;
    }
}
