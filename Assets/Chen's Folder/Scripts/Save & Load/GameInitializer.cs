using Enemies;
using Managers;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInitializer : MonoBehaviour
{
    [Header("Hero data")]
    [SerializeField] private CharacterData mainHeroData;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineCamera cinemachineFollowCamera;

    [Header("Generators")]
    [SerializeField] private DungeonLevelGenerator levelGenerator;
    [SerializeField] private RandomEnemySpawner enemySpawner;

    [Header("Managers")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PauseManager pausedManager;
    [SerializeField] private UiManager uiManager;

    public GameObject MainHero { get; private set; }
    public CharacterData ClonedHeroData { get; private set; }
    public PlayerController MainHeroController { get; private set; }

    private void Start()
    {
        ClonedHeroData = Instantiate(mainHeroData);
        StartCoroutine(InitializeGameDelayed());
    }

    private IEnumerator InitializeGameDelayed()
    {
        //an issue occurs if this one's Start is before the level generated happened. Therefore - enumerator
        while (levelGenerator.EntryPointRoom == null)
            yield return null;

        MainHero = Instantiate(ClonedHeroData.characterPrefab);
        MainHeroController = MainHero.GetComponent<PlayerController>(); //yes yes, I know, get component. The only way I found I swear
        MainHeroController.SetEntryPointAndCamera(levelGenerator.EntryPointRoom.CenterPoint, mainCamera.transform);

        // Subscribe to events
        MainHeroController.OnHit += uiManager.UpdatePlayerHpBar;
        MainHeroController.OnParry += uiManager.SetDamageMultiplierTxt;

        // Get additional components
        var playerInput = MainHero.GetComponent<PlayerInput>();
        var levelUpSystem = MainHero.GetComponent<LevelUpSystem>();

        //get the references to the level generator as well
        levelGenerator.characterObject = MainHero;
        levelGenerator.characterController = MainHeroController;

        // Load player state
        MainHeroController._curHp = DataPersistenceManager.instance.GameData.playerCurrentHealth;
        MainHeroController.LevelUpSystem.CurXp = DataPersistenceManager.instance.GameData.playerCurrentXp;
        MainHeroController.LevelUpSystem.CurrentLevel = DataPersistenceManager.instance.GameData.playerLevel;

        //here we will set the camera's following to the new hero's transform
        cinemachineFollowCamera.Follow = MainHero.transform;

        if (DataPersistenceManager.instance.HasGameData() && DataPersistenceManager.instance.GameData.PlayerPosition != Vector3.zero)
        {
            DataPersistenceManager.instance.LoadGame();
            IDataPersistence dataPersistence = MainHeroController;
            if (dataPersistence != null)
            {
                dataPersistence.LoadData(DataPersistenceManager.instance.GameData);
            }
            MainHero.transform.localPosition = DataPersistenceManager.instance.GameData.PlayerPosition;
            MainHeroController._curHp = DataPersistenceManager.instance.GameData.playerCurrentHealth;
        }
        else
        {
            DataPersistenceManager.instance.NewGame();
            enemySpawner.SpawnEnemies(levelGenerator.Level); //here we will spawn enemies if we enter to a new game.
            MainHeroController.SetEntryPointAndCamera(levelGenerator.EntryPointRoom.CenterPoint, mainCamera.transform);
            MainHeroController._curHp = MainHeroController.MaxHealth;
        }

        yield return new WaitForSeconds(0.01f);//trying a bit of delay
        enemySpawner.SetupPlayerTransform(MainHero.transform);

        SetupEnemiesReferences();
        //we setup the managers eventually
        yield return new WaitForSeconds(0.05f);
        SetupManagers(playerInput, levelUpSystem);
        enemySpawner.LoadEnemyPositions();

    }

    private void SetupEnemiesReferences()
    {
        foreach (Enemy enemy in enemySpawner.ListOfEnemies)
        {
            enemy.InitializeEnemyReferences(MainHero.transform);
        }
    }

    private void SetupManagers(PlayerInput playerInput, LevelUpSystem levelUpSystem)
    {
        gameManager.SetupGameManagerEventsAndScripts(MainHeroController, levelUpSystem);
        pausedManager.SetupPlayerScripts(playerInput);
        uiManager.SetupPlayerScripts(levelUpSystem);

        gameManager.SetupEvents();

        playerInput.actions["Pause"].performed += pausedManager.OnPause;
    }

    private void OnApplicationQuit()
    {
        // Save player state
        DataPersistenceManager.instance.GameData.playerCurrentHealth = MainHeroController._curHp;
        DataPersistenceManager.instance.GameData.playerCurrentXp = MainHeroController.LevelUpSystem.CurXp;
        DataPersistenceManager.instance.GameData.playerLevel = MainHeroController.LevelUpSystem.CurrentLevel;

    }
}
