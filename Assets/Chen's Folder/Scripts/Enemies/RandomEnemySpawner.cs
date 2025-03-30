using Enemies;
using Enemies.States;
using Managers;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyCrusherSkeletonData crusherSkeleton;
    [SerializeField] private EnemyWarriorSkeletonData warriorSkeleton;
    [SerializeField] private Camera mainCamera;

    //lists and stuff
    private List<ScriptableObject> enemyDataList;
    private List<float> spawnedEnemiesXP;
    private HashSet<Vector2Int> usedPositions;
    private List<GameObject> spawnedCrusherEnemies;
    private List<GameObject> spawnedWarriorEnemies;
    private List<Room> listOfRooms;

    private Transform playerTransform;

    public List<Enemy> ListOfEnemies { get; private set; }

    //lists to store patrol points
    private List<Vector3> crusherPatrolPoints1;
    private List<Vector3> crusherPatrolPoints2;
    private List<Vector3> warriorPatrolPoints1;
    private List<Vector3> warriorPatrolPoints2;

    private void Start()
    {
        enemyDataList = new List<ScriptableObject> { crusherSkeleton, warriorSkeleton };
        spawnedEnemiesXP = new List<float>();
        usedPositions = new HashSet<Vector2Int>();
        spawnedCrusherEnemies = new List<GameObject>();
        spawnedWarriorEnemies = new List<GameObject>();
        ListOfEnemies = new List<Enemy>();

        crusherPatrolPoints1 = new List<Vector3>();
        crusherPatrolPoints2 = new List<Vector3>();
        warriorPatrolPoints1 = new List<Vector3>();
        warriorPatrolPoints2 = new List<Vector3>();

        NextRoom.OnEnteringNextLevel += ClearEnemies;
    }

    private void OnDisable()
    {
        NextRoom.OnEnteringNextLevel -= ClearEnemies;
    }

    public void SetupPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public void SetListOfRooms(List<Room> listOfRooms, Room entryPointRoom)
    {
        this.listOfRooms = new List<Room>(listOfRooms);

        // Get a random percentage 60-80 percent of the rooms will have enemies in them.
        float randomPercentage = Random.Range(60f, 80f);
        int amountOfRoomsToSpawnEnemies = Mathf.RoundToInt(this.listOfRooms.Count * (randomPercentage / 100f));

        // Shuffle the list and get a new list with the amount of rooms to spawn enemies.
        this.listOfRooms = ShuffleList(this.listOfRooms);
        this.listOfRooms = this.listOfRooms.GetRange(0, amountOfRoomsToSpawnEnemies);
    }

    public void SpawnEnemies(int level)
    {
        int totalXP = Mathf.RoundToInt(1000 * Mathf.Pow(1.2f, level)); // 1.2 is the modifier

        // Ensure each room gets at least one enemy
        foreach (var room in listOfRooms)
        {
            totalXP -= SpawnEnemyInRoom(room, totalXP);
        }

        // Spread the remaining enemies randomly
        while (totalXP > 0 && listOfRooms.Count > 0)
        {
            totalXP -= SpawnRandomEnemy(totalXP);
        }
    }

    private int SpawnEnemyInRoom(Room room, int remainingXP)
    {
        ScriptableObject enemyData = enemyDataList[Random.Range(0, enemyDataList.Count)];
        Vector2Int spawnPosition2D = GetRandomSpawnPosition(room);
        Vector3Int spawnPosition = new Vector3Int(spawnPosition2D.x, 1, spawnPosition2D.y);

        Enemy enemyInstance;
        float enemyXP;

        if (enemyData is EnemyCrusherSkeletonData crusherData)
        {
            enemyInstance = Instantiate(crusherData.prefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
            enemyXP = crusherData.enemyScript.xp;
            spawnedCrusherEnemies.Add(enemyInstance.gameObject);
        }
        else if (enemyData is EnemyWarriorSkeletonData warriorData)
        {
            enemyInstance = Instantiate(warriorData.prefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
            enemyXP = warriorData.enemyScript.xp;
            spawnedWarriorEnemies.Add(enemyInstance.gameObject);
        }
        else
        {
            Debug.LogError("Unknown enemy data type.");
            return 0;
        }

        usedPositions.Add(spawnPosition2D);
        spawnedEnemiesXP.Add(enemyXP);

        ListOfEnemies.Add(enemyInstance);

        // Get points and add them to the enemy, also make the enemy patrol state
        Vector3 patrolPoint1 = GetRandomPointInRoom(room);
        Vector3 patrolPoint2 = GetRandomPointInRoom(room);
        EnemyPatrolState patrolState = new EnemyPatrolState(enemyInstance);
        patrolState.SetPatrolPoints(patrolPoint1, patrolPoint2);
        enemyInstance.TransitionToState(patrolState);

        // Save patrol points
        if (enemyData is EnemyCrusherSkeletonData)
        {
            crusherPatrolPoints1.Add(patrolPoint1);
            crusherPatrolPoints2.Add(patrolPoint2);
        }
        else if (enemyData is EnemyWarriorSkeletonData)
        {
            warriorPatrolPoints1.Add(patrolPoint1);
            warriorPatrolPoints2.Add(patrolPoint2);
        }

        return Mathf.RoundToInt(enemyXP);
    }

    public void InitializeEnemyReferences()
    {
        foreach (var enemy in ListOfEnemies)
        {
            enemy.InitializeEnemyReferences(playerTransform);
        }
    }

    private int SpawnRandomEnemy(int remainingXP)
    {
        Room randomRoom = listOfRooms[Random.Range(0, listOfRooms.Count)];
        return SpawnEnemyInRoom(randomRoom, remainingXP);
    }

    private Vector2Int GetRandomSpawnPosition(Room room)
    {
        Vector2Int spawnPosition;
        int attempts = 0;
        int maxAttempt = 1000;
        do
        {
            int x = Random.Range(room.bottomLeftRoomCorner.x + 2, room.topRightRoomCorner.x - 2);
            int y = Random.Range(room.bottomLeftRoomCorner.y + 2, room.topRightRoomCorner.y - 2);
            spawnPosition = new Vector2Int(x, y);
            attempts++;
        } while (usedPositions.Contains(spawnPosition) && attempts < maxAttempt);

        if (attempts >= maxAttempt)//just to be safe yknow? 
        {
            Debug.LogError("No available positions to spawn enemy in room.");
            return Vector2Int.RoundToInt(room.CenterPoint);//if we made too many enemies, they will spawn in the cetner. Just in case
        }

        return spawnPosition;
    }

    private Vector3 GetRandomPointInRoom(Room room)
    {
        int x = Random.Range(room.bottomLeftRoomCorner.x + 1, room.topRightRoomCorner.x - 1);
        int y = Random.Range(room.bottomLeftRoomCorner.y + 1, room.topRightRoomCorner.y - 1);
        return new Vector3(x, 1, y);
    }

    private List<Room> ShuffleList(List<Room> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Room temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    public void SaveEnemyPositions()
    {
        GameData gameData = DataPersistenceManager.instance.GameData;
        DataPersistenceManager.instance.GameData.crusherSkeletonPositions.Clear();
        DataPersistenceManager.instance.GameData.warriorSkeletonPositions.Clear();
        DataPersistenceManager.instance.GameData.crusherPatrolPoints1.Clear();
        DataPersistenceManager.instance.GameData.crusherPatrolPoints2.Clear();
        DataPersistenceManager.instance.GameData.warriorPatrolPoints1.Clear();
        DataPersistenceManager.instance.GameData.warriorPatrolPoints2.Clear();

        foreach (GameObject enemy in spawnedCrusherEnemies)
        {
            DataPersistenceManager.instance.GameData.crusherSkeletonPositions.Add(enemy.transform.position);
        }

        foreach (GameObject enemy in spawnedWarriorEnemies)
        {
            DataPersistenceManager.instance.GameData.warriorSkeletonPositions.Add(enemy.transform.position);
        }

        DataPersistenceManager.instance.GameData.crusherPatrolPoints1.AddRange(crusherPatrolPoints1);
        DataPersistenceManager.instance.GameData.crusherPatrolPoints2.AddRange(crusherPatrolPoints2);
        DataPersistenceManager.instance.GameData.warriorPatrolPoints1.AddRange(warriorPatrolPoints1);
        DataPersistenceManager.instance.GameData.warriorPatrolPoints2.AddRange(warriorPatrolPoints2);

        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadEnemyPositions()
    {
        var data = DataPersistenceManager.instance.GameData;

        // Safety checks for null or empty
        bool hasCrushers = data.crusherSkeletonPositions != null && data.crusherSkeletonPositions.Count > 0;
        bool hasWarriors = data.warriorSkeletonPositions != null && data.warriorSkeletonPositions.Count > 0;

        bool hasCrusherPatrols = data.crusherPatrolPoints1 != null && data.crusherPatrolPoints2 != null &&
                                 data.crusherPatrolPoints1.Count == data.crusherSkeletonPositions.Count &&
                                 data.crusherPatrolPoints2.Count == data.crusherSkeletonPositions.Count;

        bool hasWarriorPatrols = data.warriorPatrolPoints1 != null && data.warriorPatrolPoints2 != null &&
                                 data.warriorPatrolPoints1.Count == data.warriorSkeletonPositions.Count &&
                                 data.warriorPatrolPoints2.Count == data.warriorSkeletonPositions.Count;

        if (!hasCrushers) data.crusherSkeletonPositions = new List<Vector3>();
        if (!hasWarriors) data.warriorSkeletonPositions = new List<Vector3>();
        if (!hasCrusherPatrols)
        {
            data.crusherPatrolPoints1 = new List<Vector3>();
            data.crusherPatrolPoints2 = new List<Vector3>();
        }
        if (!hasWarriorPatrols)
        {
            data.warriorPatrolPoints1 = new List<Vector3>();
            data.warriorPatrolPoints2 = new List<Vector3>();
        }

        for (int i = 0; i < data.crusherSkeletonPositions.Count; i++)
        {
            GameObject enemy = Instantiate(crusherSkeleton.prefab, data.crusherSkeletonPositions[i], Quaternion.identity);
            spawnedCrusherEnemies.Add(enemy);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            ListOfEnemies.Add(enemyScript);

            if (hasCrusherPatrols)
            {
                Vector3 patrolPoint1 = data.crusherPatrolPoints1[i];
                Vector3 patrolPoint2 = data.crusherPatrolPoints2[i];
                var patrolState = new EnemyPatrolState(enemyScript);
                patrolState.SetPatrolPoints(patrolPoint1, patrolPoint2);
                enemyScript.TransitionToState(patrolState);
            }
        }

        for (int i = 0; i < data.warriorSkeletonPositions.Count; i++)
        {
            GameObject enemy = Instantiate(warriorSkeleton.prefab, data.warriorSkeletonPositions[i], Quaternion.identity);
            spawnedWarriorEnemies.Add(enemy);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            ListOfEnemies.Add(enemyScript);

            if (hasWarriorPatrols)
            {
                Vector3 patrolPoint1 = data.warriorPatrolPoints1[i];
                Vector3 patrolPoint2 = data.warriorPatrolPoints2[i];
                var patrolState = new EnemyPatrolState(enemyScript);
                patrolState.SetPatrolPoints(patrolPoint1, patrolPoint2);
                enemyScript.TransitionToState(patrolState);
            }
        }

        InitializeEnemyReferences();

    }

    public void ClearEnemies()
    {
        foreach (var enemy in ListOfEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        ListOfEnemies.Clear();
        spawnedCrusherEnemies.Clear();
        spawnedWarriorEnemies.Clear();
        usedPositions.Clear();
        spawnedEnemiesXP.Clear();
        crusherPatrolPoints1.Clear();
        crusherPatrolPoints2.Clear();
        warriorPatrolPoints1.Clear();
        warriorPatrolPoints2.Clear();
    }
}

