using System.Collections.Generic;
using UnityEngine;

namespace Chen_s_Folder.Scripts.Save___Load.Data
{
    [System.Serializable]
    public class GameData
    {
        //our seed if we have
        public int currentDungeonSeed;
        public long lastUpdated; 
        public bool useRandomSeed;


        //player variables
        public Vector3 playerPosition;
        public int dungeonLevel;
        public float playerCurrentHealth;
        public int playerLevel;
        public float playerCurrentXp;

        //all of the enemies lists, one per type
        public List<Vector3> crusherSkeletonPositions;
        public List<Vector3> warriorSkeletonPositions;

        public List<Vector3> crusherPatrolPoints1 = new List<Vector3>();
        public List<Vector3> crusherPatrolPoints2 = new List<Vector3>();
        public List<Vector3> warriorPatrolPoints1 = new List<Vector3>();
        public List<Vector3> warriorPatrolPoints2 = new List<Vector3>();


        public GameData()
        {
            currentDungeonSeed = 0;
            useRandomSeed = true;
            playerPosition = Vector3.zero;
            dungeonLevel = 1;
            playerCurrentXp = 0f;

            crusherSkeletonPositions = new List<Vector3>();
            warriorSkeletonPositions = new List<Vector3>();

        }

    }

    [System.Serializable]
    public class CrusherSaveData
    {
        public Vector3 position;
    }

    [System.Serializable]
    public class WarriorSaveData
    {
        public Vector3 position;
    }
}