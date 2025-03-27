using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //our seed if we have
    public int currentDungeonSeed;
    public long lastUpdated; 
    public bool useRandomSeed;


    //player varaiables
    public Vector3 PlayerPosition;

    public GameData()
    {
        currentDungeonSeed = 0;
        useRandomSeed = true;
        PlayerPosition = Vector3.zero;
    }

}
