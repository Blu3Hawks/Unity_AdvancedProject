using UnityEngine;
using Enemies;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public Enemy enemyScript;
}
