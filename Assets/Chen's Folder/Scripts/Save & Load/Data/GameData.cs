using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentDungeonSeed;
    public long lastUpdated;
    public Vector3 characterPosition;



    public GameData()
    {
        lastUpdated = System.DateTime.Now.ToBinary();
    }
}
