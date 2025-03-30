using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chen_s_Folder.Scripts.Save___Load.Data
{
    public class DataFiles : MonoBehaviour, IDataPersistence
    {
        private static DataFiles OnlyDataFiles;

        public int currentSceneSeed;

        //camera position
        public Vector3 cameraPos;

        private void Awake()
        {
            if (OnlyDataFiles != null && OnlyDataFiles == this)
            {
                Destroy(this.gameObject);
                return;
            }
            if (OnlyDataFiles == null)
            {
                OnlyDataFiles = this;
                DontDestroyOnLoad(this.gameObject);
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //on enable - we will load them 
        private void OnEnable()
        {
            SetBuildings();
        }

        //same when we load the scene
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetBuildings();
        }

        //what data do we save
        public void SaveData(GameData data)
        {
            data.currentDungeonSeed = currentSceneSeed;
        }

        //what data do we load
        public void LoadData(GameData data)
        {
            currentSceneSeed = data.currentDungeonSeed;
        }

        //what do we need to setup, and what's their attributes:
        private void SetBuildings()
        {

        }
    }
}
