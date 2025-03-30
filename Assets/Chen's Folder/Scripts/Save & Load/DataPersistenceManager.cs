using System.Collections.Generic;
using System.Linq;
using Chen_s_Folder.Scripts.Save___Load.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chen_s_Folder.Scripts.Save___Load
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }

        private GameData _gameData;

        private List<IDataPersistence> _dataPersistenceObjects;

        [SerializeField] private bool useEncryption;
        [SerializeField] private bool initializeDataIfNull = false;
        //debugging
        [SerializeField] private bool disableDataPersistence = false;
        //saving overrides
        [SerializeField] private bool overrideSelectedProfileId = false;
        [SerializeField] private string testSelectedProfileId = "test";
        [SerializeField] private string fileName;

        private string _selectedProfileId = "";
        private FileDataHandler _dataHandler;

        public GameData GameData { get { return _gameData; } }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("there's already an instance");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            if (disableDataPersistence)
            {
                Debug.LogWarning("Data Persistnce Manager is disabled !");
            }
            this._dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
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
            this._dataPersistenceObjects = FindAllDataPersistenceObjects();
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
            _gameData = new GameData();
            SaveGame();
        }

        public void LoadGame()
        {
            if (disableDataPersistence)
            {
                return;
            }
            this._gameData = _dataHandler.Load(_selectedProfileId);

            if (this._gameData == null && initializeDataIfNull)
            {
                NewGame();
            }
            if (this._gameData == null)
            {
                Debug.Log("There's no saved game to be loaded - start a new game");
                return;
            }

            //in case we've found game data (game data != null)
            foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            if (disableDataPersistence)
            {
                return;
            }
            if (this._gameData == null)
            {
                Debug.Log("For some reason we can't save this null file");
                return;
            }
            foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(_gameData);
            }
            //the most recent game that we've used is going to be on top - which means the most recent.
            _gameData.lastUpdated = System.DateTime.Now.ToBinary();
            _dataHandler.Save(_gameData, _selectedProfileId);
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            //update the profile for saving/loading
            this._selectedProfileId = newProfileId;
            //load the game while using the profile Id
            LoadGame();
        }

        public void DeleteProfileId(string profileId)
        {
            //delete the data for this profile
            _dataHandler.Delete(profileId);
            //initialize the selected profile
            InitializeSelectedProfileId();
            //reload the game so that our data matches the newly selected profile
            LoadGame();
        }

        private void InitializeSelectedProfileId()
        {
            this._selectedProfileId = _dataHandler.GetMostRecentProfileId();
            if (overrideSelectedProfileId)
            {
                this._selectedProfileId = testSelectedProfileId;
                Debug.LogWarning("Overriding a selected profile Id with test: " + testSelectedProfileId);
            }
        }

        public bool HasGameData()
        {
            return _gameData != null;
        }

        public Dictionary<string, GameData> GetAllProfilesData()
        {
            return _dataHandler.LoadAllProfiles();
        }

        public GameData GetSavedGameData()
        {
            return _gameData;
        }
    }
}
