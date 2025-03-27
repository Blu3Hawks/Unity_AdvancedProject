using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject mainHeroPrefab;
    private Vector3 defaultHeroPosition = Vector3.zero;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Instantiate the MainHero
        GameObject mainHero = Instantiate(mainHeroPrefab, defaultHeroPosition, Quaternion.identity);

        // Check if there is saved data
        if (DataPersistenceManager.instance.HasGameData())
        {
            // Load the saved data
            DataPersistenceManager.instance.LoadGame();

            // Apply the saved data to the MainHero
            IDataPersistence dataPersistence = mainHero.GetComponent<IDataPersistence>();
            if (dataPersistence != null)
            {
                dataPersistence.LoadData(DataPersistenceManager.instance.GameData);
            }
        }
        else
        {
            // No saved data, start a new game
            DataPersistenceManager.instance.NewGame();
        }
    }
}
