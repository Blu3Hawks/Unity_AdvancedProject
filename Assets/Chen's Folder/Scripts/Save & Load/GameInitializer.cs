using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject mainHeroPrefab;
    [SerializeField] private DungeonLevelGenerator levelGenerator;
    [SerializeField] private RandomEnemySpawner enemySpawner;

    private void Start()
    {
        StartCoroutine(InitializeGameDelayed());
    }

    private IEnumerator InitializeGameDelayed()
    {
        //an issue occurs if this one's STart is before the level generated happened. Therefore - enumerator
        while (levelGenerator.EntryPointRoom == null)
            yield return null;

        Vector3 entryPointPosition = new Vector3(levelGenerator.EntryPointRoom.CenterPoint.x, 1f, levelGenerator.EntryPointRoom.CenterPoint.y + 2);
        GameObject mainHeroObject = Instantiate(mainHeroPrefab, Vector3.zero, Quaternion.identity);
        MainHero mainHero = mainHeroObject.GetComponent<MainHero>();
        mainHero.entryPointPosition = entryPointPosition;

        if (DataPersistenceManager.instance.HasGameData() && DataPersistenceManager.instance.GameData.PlayerPosition != Vector3.zero)
        {
            DataPersistenceManager.instance.LoadGame();
            mainHero.transform.position = DataPersistenceManager.instance.GameData.PlayerPosition;
            IDataPersistence dataPersistence = mainHero.GetComponent<IDataPersistence>();
            if (dataPersistence != null)
            {
                dataPersistence.LoadData(DataPersistenceManager.instance.GameData);
            }
        }
        else
        {
            DataPersistenceManager.instance.NewGame();
            enemySpawner.SpawnEnemies(levelGenerator.Level); //here we will spawn enemies if we enter to a new game.

            mainHero.transform.position = entryPointPosition;
        }
    }

    

}
