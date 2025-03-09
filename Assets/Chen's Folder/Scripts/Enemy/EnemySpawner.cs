using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //configurations for the whole level - how many waves basically
    public List<WaveConfiguration> configurations;
    //our current wave
    private int currentWave = 0;

    //needs to be called in start only, for now
    private void Start()
    {
        StartCoroutine(SpawnLevel());
    }

    //coroutine for the whole level - all the waves and their single spawns
    private IEnumerator SpawnLevel()
    {
        while (currentWave < configurations.Count)
        {
            yield return StartCoroutine(SpawnWave(configurations[currentWave]));
            yield return new WaitForSeconds(configurations[currentWave].timeBetweenWaves);
            currentWave++;
        }
    }

    //coroutine for a wave of enemies
    private IEnumerator SpawnWave(WaveConfiguration waveConfig)
    {
        foreach (SingleSpawn singleSpawn in waveConfig.waves)
        {
            yield return StartCoroutine(SingleSpawn(singleSpawn));
            yield return new WaitForSeconds(singleSpawn.timeBetweenSpawns);
        }
    }

    //coroutine for a single spawn
    private IEnumerator SingleSpawn(SingleSpawn singleSpawn)
    {
        for (int i = 0; i < singleSpawn.amountOfEnemies; i++)
        {
            Instantiate(singleSpawn.enemyPrefab, transform.position, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(singleSpawn.spawnTimer);
        }
    }
}


//what a single spawn looks like and contains
[System.Serializable]
public class SingleSpawn
{
    public GameObject enemyPrefab;
    public int amountOfEnemies;
    public float spawnTimer;
    [Space]
    public float timeBetweenSpawns;
}

//what a wave configuration looks like and contains - which is a bunch of single spawns
[System.Serializable]
public class WaveConfiguration
{
    public List<SingleSpawn> waves = new List<SingleSpawn>();
    public float timeBetweenWaves;
}