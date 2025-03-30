using Enemies;
using UnityEngine;

namespace Chen_s_Folder.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Enemy Crusher Skeleton Data", menuName = "Scriptable Objects/Enemy Crusher Skeleton Data", order = 2)]
    public class EnemyCrusherSkeletonData : ScriptableObject
    {
        public GameObject prefab;
        public CrusherSkeletonEnemy enemyScript;
    }
}
