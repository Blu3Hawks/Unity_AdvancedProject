using UnityEngine;
using UnityEditor;
using Enemies;

[CreateAssetMenu(fileName = "Enemy Crusher Skeleton Data", menuName = "Scriptable Objects/Enemy Crusher Skeleton Data", order = 2)]
public class EnemyCrusherSkeletonData : ScriptableObject
{
    public GameObject prefab;
    public CrusherSkeletonEnemy enemyScript;
}
