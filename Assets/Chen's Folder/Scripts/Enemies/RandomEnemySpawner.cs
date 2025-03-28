using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
    
    private List<Room> listOfRooms;
    [SerializeField] private List<EnemyData> enemyDataList;

    public void SetListOfRooms(List<Room> listOfRooms, Room entryPointRoom)
    {
        this.listOfRooms = new List<Room>(listOfRooms);
        this.listOfRooms.Remove(entryPointRoom);

        //get a random percentage 60-80 percent of the rooms will have enemies in them. 
        float randomPercentage = Random.Range(60f, 80f);
        int amountOfRoomsToSpawnEnemies = Mathf.RoundToInt(this.listOfRooms.Count * (randomPercentage / 100f));

        //Next let's do some shuffling to get a new list - the amount of rooms 
        this.listOfRooms = ShuffleList(this.listOfRooms);
        this.listOfRooms = this.listOfRooms.GetRange(0, amountOfRoomsToSpawnEnemies);
    }

    public void SpawnEnemies(int level)
    {
        int totalXP = Mathf.RoundToInt(1000 * Mathf.Pow(1.2f, level)); //1.2 is the modifier
        Debug.Log("Total XP for level " + level + ": " + totalXP);

        while (totalXP > 0 && listOfRooms.Count > 0)
        {
            totalXP -= SpawnRandomEnemy(totalXP);
        }
    }
    private int SpawnRandomEnemy(int remainingXP)
    {
        EnemyData enemyData = enemyDataList[Random.Range(0, enemyDataList.Count)];
        Room randomRoom = listOfRooms[Random.Range(0, listOfRooms.Count)];
        Vector2 spawnPosition = randomRoom.CenterPoint; // for now it's the center point of the room. Not for long tho

        GameObject enemyInstance = Instantiate(enemyData.prefab, spawnPosition, Quaternion.identity);

        // use the xp from the enemy data's script.
        float enemyXP = enemyData.enemyScript.xp;
        Debug.Log("Spawned a " + enemyData.enemyScript.GetType().Name + " with XP: " + enemyXP);

        return Mathf.RoundToInt(enemyXP);
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
}

