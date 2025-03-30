using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    [SerializeField] private bool useEncryption;
    [SerializeField] private bool initializeDataIfNull = false;
    //debugging
    [SerializeField] private bool disableDataPersistence = false;
    //saving overrides
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    [SerializeField] private string fileName;

    private string selectedProfileId = "";
    private FileDataHandler dataHandler;

    public GameData GameData { get { return gameData; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("there's already an instance");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistnce Manager is disabled !");
        }
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public void NewGame()
    {
        gameData = new GameData();
        SaveGame();
    }

    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            return;
        }
        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        if (this.gameData == null)
        {
            Debug.Log("There's no saved game to be loaded - start a new game");
            return;
        }

        //in case we've found game data (game data != null)
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }
        if (this.gameData == null)
        {
            Debug.Log("For some reason we can't save this null file");
            return;
        }
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(gameData);
        }
        //the most recent game that we've used is going to be on top - which means the most recent.
        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        dataHandler.Save(gameData, selectedProfileId);
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        //update the profile for saving/loading
        this.selectedProfileId = newProfileId;
        //load the game while using the profile Id
        LoadGame();
    }

    public void DeleteProfileId(string profileId)
    {
        //delete the data for this profile
        dataHandler.Delete(profileId);
        //initialize the selected profile
        InitializeSelectedProfileId();
        //reload the game so that our data matches the newly selected profile
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overriding a selected profile Id with test: " + testSelectedProfileId);
        }
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public GameData GetSavedGameData()
    {
        return gameData;
    }
}
