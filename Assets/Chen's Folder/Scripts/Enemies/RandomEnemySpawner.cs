using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
    private List<Room> listOfRooms;

    public void SetListOfRooms(List<Room> listOfRooms)
    {
        this.listOfRooms = listOfRooms;
    }

    public void SpawnEnemies(int level)
    {
        int totalXP = Mathf.RoundToInt(1000 * Mathf.Pow(1.2f, level));
        Debug.Log("Total XP for level " + level + ": " + totalXP);


    }
}

