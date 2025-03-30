using System.Collections;
using System.Collections.Generic;
using Chen_s_Folder.Scripts.Enemies;
using Chen_s_Folder.Scripts.Load_Next_Scenes;
using Chen_s_Folder.Scripts.Save___Load;
using Chen_s_Folder.Scripts.Save___Load.Data;
using UnityEngine;
using random = UnityEngine.Random;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class DungeonLevelGenerator : MonoBehaviour
    {
        [Header("Level Values")]
        [SerializeField] private int minLevelWidth;
        [SerializeField] private int maxLevelWidth;
        private int _levelWidth;

        [SerializeField] private int minLevelLength;
        [SerializeField] private int maxLevelLength;
        private int _levelLength;

        [SerializeField] private int minIterations;
        [SerializeField] private int maxIterations;
        private int _levelIterations;

        [Header("Room Values")]
        [SerializeField] private int minRoomWidth;
        [SerializeField] private int maxRoomWidth;

        [SerializeField] private int minRoomLength;
        [SerializeField] private int maxRoomLength;

        [Header("Hallway Values")]
        [SerializeField] private float hallwayWidth;
        [SerializeField] private GameObject hallwayPrefab;

        [SerializeField] private GameObject planePrefab;
        [SerializeField] private GameObject roomPlanePrefab;

        [Header("Walls Values")]
        [SerializeField] private GameObject wallPrefab;

        [Header("Start & End point")]
        [SerializeField] private GameObject entryPoint;
        [SerializeField] private GameObject exitPoint;

        [Header("Spawners")]
        [SerializeField] private RandomEnemySpawner enemySpawner;
        [SerializeField] private GameInitializer gameInitializer;

        public int seed;
        private bool _useRandomSeed = true;
        private int _level;

        //references
        private SpaceDivider _spaceDivider;

        //private lists
        private List<Room> _listOfRooms = new List<Room>();
        private List<Hallway> _listOfHallways = new List<Hallway>();
        private List<GameObject> _listOfHallwaysObjects = new List<GameObject>();

        private RoomConnector _roomConnector;
        private WallsInitializer _wallsInitializer;

        //parent objects
        private Transform _spacesParent;
        private Transform _roomsParent;
        private Transform _hallwaysParent;
        private Transform _wallParent;

        //local objects
        private Room _currentEntryRoom;
        private GameData _gameData;

        //player references
        public GameObject characterObject;
        public PlayerController characterController;


        //properties
        public Room EntryPointRoom { get { return _currentEntryRoom; } } //the player will access it - and spawn on top of it
        public int Level { get { return _level; } }

        private void Start()
        {
            _gameData = DataPersistenceManager.Instance.GetSavedGameData();
            if (_gameData != null)
            {
                seed = _gameData.currentDungeonSeed;
                _useRandomSeed = _gameData.useRandomSeed;
                Debug.Log("Loaded seed: " + _gameData.currentDungeonSeed);
                Debug.Log("Loaded useRandomSeed: " + _gameData.useRandomSeed);
            }
            else
            {
                _useRandomSeed = true;
            }
            GenerateLevelWithSeed();
            Debug.Log($"[Seed Used]: {seed}");
            NextRoom.OnEnteringNextLevel += HandleEnteringNextLevel;
            StartCoroutine(SetupAllEnemiesReferences());
        }


        private IEnumerator SetupAllEnemiesReferences()
        {
            yield return new WaitForEndOfFrame();
            characterObject = gameInitializer.MainHero;

            enemySpawner.InitializeEnemyReferences();
        }

        private void OnDestroy()
        {
            NextRoom.OnEnteringNextLevel -= HandleEnteringNextLevel;
        }

        private void HandleEnteringNextLevel()
        {
            _useRandomSeed = true;
            _level = DataPersistenceManager.Instance.GameData.dungeonLevel; // get the level saved.
            DataPersistenceManager.Instance.GameData.currentDungeonSeed = seed;

            _level++;

            seed = random.Range(int.MinValue, int.MaxValue); // get new seed

            // generate a new level
            GenerateLevel();

            // update the hero's position to the new entry point
            characterObject.transform.position = new Vector3(_currentEntryRoom.CenterPoint.x, characterObject.transform.position.y, _currentEntryRoom.CenterPoint.x + 2);

            List<Room> newListOfRooms = new List<Room>(_listOfRooms);
            newListOfRooms.Remove(EntryPointRoom);
            enemySpawner.SetListOfRooms(newListOfRooms, EntryPointRoom);
            enemySpawner.SpawnEnemies(_level); // here we will spawn them enemieessss only once we enter a new wave tho.
        }

        public void GenerateLevel()
        {
            ClearWorldInGame(); //clear everything previously made
            GenerateRandomLevelValues(); //get bew random values
            _spaceDivider = new SpaceDivider(_levelWidth, _levelLength, _levelIterations, maxRoomWidth, maxRoomLength);//divide the space
            _spaceDivider.GenerateAllSpaces();//generate them

            foreach (SpaceArea space in _spaceDivider.SpacesToPrint)
            {
                GenerateRoom(space); //generate new rooms for each space
            }

            _roomConnector = new RoomConnector(_listOfRooms);//create a room connector

            CreatePlanesForSpaces();//initialize meshes
            CreatePlanesForRooms();//same here
            CreatePlanesForHallways(_roomConnector);//hallways, corridors w/e

            _wallsInitializer = new WallsInitializer();//create walls
            foreach (Room room in _listOfRooms)
            {
                _wallsInitializer.SetupWallsForRoom(room, 1f);
                _wallsInitializer.RemoveAllOverlappingWalls(_listOfRooms, _listOfHallwaysObjects);
                CreateWallsForRooms();
            }

            foreach (GameObject hallwayObject in _listOfHallwaysObjects)
            {
                _wallsInitializer.SetupWallsForHallway(hallwayObject, 1f, _listOfRooms, _listOfHallways);
                _wallsInitializer.RemoveAllOverlappingWalls(_listOfRooms, _listOfHallwaysObjects);
                CreateWallsForHallways();
            }

            // we will check if, somehow, we have a room with no connections
            foreach (Room room in _listOfRooms)
            {
                if (room.AmountOfRoomsToConnect <= 0)
                {
                    GenerateLevel();
                }
            }

            //set the enemy spawner's list of rooms
            List<Room> newListOfRooms = new List<Room>(_listOfRooms);
            newListOfRooms.Remove(EntryPointRoom);
            enemySpawner.SetListOfRooms(newListOfRooms, EntryPointRoom);
            DeclareStartingAndExitRooms();
            DataPersistenceManager.Instance.GameData.useRandomSeed = false;

        }

        public void SetUseRandomSeed(bool useRandomSeed)
        {
            this._useRandomSeed = useRandomSeed;
        }

        private void GenerateRandomLevelValues()
        {
            if (seed == 0)
            {
                seed = random.Range(int.MinValue, int.MaxValue);
            }

            random.InitState(seed);

            _levelWidth = random.Range(minLevelWidth, maxLevelWidth);
            _levelLength = random.Range(minLevelLength, maxLevelLength);
            _levelIterations = random.Range(minIterations, maxIterations);
        }

        private void CreateWallsForHallways()
        {
            foreach (Vector2 wallPosition in _wallsInitializer.ListOfWallsForHallways)
            {
                if (wallPrefab == null)
                {
                    Debug.LogError("Wall prefab is not assigned!");
                    return;
                }
                float yScale = wallPrefab.transform.localScale.y;
                Vector3 adjustedPosition = new Vector3(wallPosition.x, yScale / 2, wallPosition.y);

                GameObject wall = Instantiate(wallPrefab, _wallParent);

                wall.transform.position = adjustedPosition;
            }
        }

        private void CreateWallsForRooms()
        {
            foreach (Vector2 wallPosition in _wallsInitializer.ListOfWallsForRooms)
            {
                if (wallPrefab == null)
                {
                    Debug.LogError("Wall prefab is not assigned!");
                    return;
                }

                float yScale = wallPrefab.transform.localScale.y;
                Vector3 adjustedPosition = new Vector3(wallPosition.x, yScale / 2, wallPosition.y);

                GameObject wall = Instantiate(wallPrefab, _wallParent);

                wall.transform.position = adjustedPosition;
            }
        }

        private void CreatePlanesForSpaces()
        {
            foreach (SpaceArea space in _spaceDivider.SpacesToPrint)
            {
                if (planePrefab == null)
                {
                    Debug.LogError("Plane prefab is not assigned!");
                    return;
                }

                GameObject plane = Instantiate(planePrefab, _spacesParent);

                Vector3 position = new Vector3(
                    (space.BottomLeftSpaceCorner.x + space.TopRightSpaceCorner.x) / 2f,
                    0,
                    (space.BottomLeftSpaceCorner.y + space.TopRightSpaceCorner.y) / 2f
                );

                Vector3 scale = new Vector3(
                    space.Width / 10f,
                    1,
                    space.Length / 10f
                );

                plane.transform.position = position;
                plane.transform.localScale = scale;
            }
        }

        private void CreatePlanesForRooms()
        {
            foreach (Room room in _listOfRooms)
            {
                if (roomPlanePrefab == null)
                {
                    Debug.LogError("Room plane prefab is not assigned!");
                    return;
                }

                GameObject roomPlane = Instantiate(roomPlanePrefab, _roomsParent);

                Vector3 position = new Vector3(
                    (room.BottomLeftRoomCorner.x + room.TopRightRoomCorner.x) / 2f,
                    1,
                    (room.BottomLeftRoomCorner.y + room.TopRightRoomCorner.y) / 2f
                );

                Vector3 scale = new Vector3(
                    room.Width / 10f,
                    1,
                    room.Length / 10f
                );

                roomPlane.transform.position = position;
                roomPlane.transform.localScale = scale;
            }
        }

        public void GenerateRoom(SpaceArea givenSpace)
        {
            int randomRoomWidth = random.Range(minRoomWidth, maxRoomWidth);
            int randomRoomLength = random.Range(minRoomLength, maxRoomLength);

            Vector2Int newBottomLeftCorner = new Vector2Int(
                random.Range(givenSpace.BottomLeftSpaceCorner.x + 1, givenSpace.BottomRightSpaceCorner.x - 1 - randomRoomWidth),
                random.Range(givenSpace.BottomLeftSpaceCorner.y + 1, givenSpace.TopLeftSpaceCorner.y - 1 - randomRoomLength)
            );

            Vector2Int newTopRightCorner = new Vector2Int(
                newBottomLeftCorner.x + randomRoomWidth,
                newBottomLeftCorner.y + randomRoomLength
            );

            int randomAmountOfRoomsToConnect = random.Range(1, 4);

            Room newRoom = new Room(newBottomLeftCorner, newTopRightCorner, randomAmountOfRoomsToConnect);
            _listOfRooms.Add(newRoom);
        }

        private void CreatePlanesForHallways(RoomConnector roomConnector)
        {
            // have a hashset for the pairs of rooms, so every 2 rooms that are connected considered paired
            HashSet<(Room, Room)> connectedPairs = new HashSet<(Room, Room)>();

            foreach (Room room in _listOfRooms)
            {
                foreach (Room connected in room.ListOfRoomsToConnect)
                {
                    // make sure we don't repeat the same rooms
                    if (connectedPairs.Contains((connected, room)) || connectedPairs.Contains((room, connected)))
                        continue;

                    // add them to the hashset and instantiate
                    connectedPairs.Add((room, connected));
                    InstantiateHallwayBetweenRooms(room, connected, roomConnector);
                }
            }
        }

        private void InstantiateHallwayBetweenRooms(Room roomA, Room roomB, RoomConnector roomConnector)
        {
            // check starting and lasting rooms' center points
            Vector2 startPoint = roomConnector.GetClosestSidePoint(roomA, roomB.CenterPoint);
            Vector2 endPoint = roomConnector.GetClosestSidePoint(roomB, roomA.CenterPoint);

            // start by horizontally instantiation
            Vector2 horizontal = new Vector2(endPoint.x, startPoint.y);
            SpawnHallway(startPoint, horizontal);
            // then move vertically
            Vector2 vertical = endPoint;
            SpawnHallway(horizontal, vertical);
        }

        // here we get a start point and an end point, by getting these values,
        // we ensure that we will be moving properly horizontally first and then vertically
        private void SpawnHallway(Vector2 startPoint, Vector2 endPoint)
        {
            if (hallwayPrefab == null)
            {
                Debug.LogError("Hallway prefab not assigned!");
                return;
            }

            Vector2 midPoint = (startPoint + endPoint) / 2;
            Vector2 direction = endPoint - startPoint;
            float length = direction.magnitude;

            Vector3 position = new Vector3(midPoint.x, 0.99f, midPoint.y);
            Vector3 scale;

            float scaleModifier = 9.15f;

            // check the direction that we will be facing, and scale it properly towards where we're heading
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                scale = new Vector3(length / scaleModifier, 1f, hallwayWidth / scaleModifier);
            }
            else
            {
                scale = new Vector3(hallwayWidth / scaleModifier, 1f, length / scaleModifier);
            }

            // instantiate the hallway GameObject
            GameObject hallwayObject = Instantiate(hallwayPrefab, position, Quaternion.identity, _hallwaysParent);
            hallwayObject.transform.localScale = scale;
            _listOfHallwaysObjects.Add(hallwayObject);

            // Create hallway and add it to the list of hallways
            Vector2Int bottomLeftCorner = Vector2Int.RoundToInt(startPoint);
            Vector2Int topRightCorner = Vector2Int.RoundToInt(endPoint);
            Hallway newHallway = new Hallway(bottomLeftCorner, topRightCorner);
            _listOfHallways.Add(newHallway);
        }

        // here we are going to add a starting and ending point. So the game will choose a random room, every time, one for entry and one for exit.
        // we will have the object themselves decide what room

        private void DeclareStartingAndExitRooms()
        {
            Room entryRoom;
            Room exitRoom;

            List<Room> allRooms = _listOfRooms;
            int randomEntryRoomIndex = random.Range(0, allRooms.Count);
            int randomExitRoomIndex = random.Range(0, allRooms.Count);

            while (randomEntryRoomIndex == randomExitRoomIndex)
            {
                randomExitRoomIndex = random.Range(0, allRooms.Count);
            }

            entryRoom = allRooms[randomEntryRoomIndex];
            exitRoom = allRooms[randomExitRoomIndex];

            // Set the current entry object to the selected entry room
            _currentEntryRoom = entryRoom;

            // now we will decide where to place the entry point and exit point. both will be in the middle of the room
            Instantiate(entryPoint, new Vector3(entryRoom.CenterPoint.x, 1f, entryRoom.CenterPoint.y), Quaternion.identity, _roomsParent);
            GameObject exitLevelPoint = Instantiate(exitPoint, new Vector3(exitRoom.CenterPoint.x, 1f, exitRoom.CenterPoint.y), Quaternion.identity, _roomsParent);
        }

        private void ClearWorldInGame()
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }

            if (_spacesParent != null)
            {
                foreach (Transform child in _spacesParent)
                {
                    Destroy(child.gameObject);
                }
                Destroy(_spacesParent.gameObject);
            }

            if (_roomsParent != null)
            {
                foreach (Transform child in _roomsParent)
                {
                    Destroy(child.gameObject);
                }
                Destroy(_roomsParent.gameObject);
            }

            if (_hallwaysParent != null)
            {
                foreach (Transform child in _hallwaysParent)
                {
                    Destroy(child.gameObject);
                }
                Destroy(_hallwaysParent.gameObject);
            }

            if (_wallParent != null)
            {
                foreach (Transform child in _wallParent)
                {
                    Destroy(child.gameObject);
                }
                Destroy(_wallParent.gameObject);
            }

            _spacesParent = new GameObject("Spaces").transform;
            _roomsParent = new GameObject("Rooms").transform;
            _hallwaysParent = new GameObject("Hallways").transform;
            _wallParent = new GameObject("Walls").transform;

            _spacesParent.SetParent(transform);
            _roomsParent.SetParent(transform);
            _hallwaysParent.SetParent(transform);
            _wallParent.SetParent(transform);

            _listOfRooms.Clear();
            _listOfHallways.Clear();
            _listOfHallwaysObjects.Clear();
        }

        public void ClearWorldInEditor()
        {
            foreach (Transform child in this.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            if (_spacesParent != null)
            {
                foreach (Transform child in _spacesParent)
                {
                    DestroyImmediate(child.gameObject);
                }
                DestroyImmediate(_spacesParent.gameObject);
            }

            if (_roomsParent != null)
            {
                foreach (Transform child in _roomsParent)
                {
                    DestroyImmediate(child.gameObject);
                }
                DestroyImmediate(_roomsParent.gameObject);
            }

            if (_hallwaysParent != null)
            {
                foreach (Transform child in _hallwaysParent)
                {
                    DestroyImmediate(child.gameObject);
                }
                DestroyImmediate(_hallwaysParent.gameObject);
            }

            if (_wallParent != null)
            {
                foreach (Transform child in _wallParent)
                {
                    DestroyImmediate(child.gameObject);
                }
                DestroyImmediate(_wallParent.gameObject);
            }

            _spacesParent = new GameObject("Spaces").transform;
            _roomsParent = new GameObject("Rooms").transform;
            _hallwaysParent = new GameObject("Hallways").transform;
            _wallParent = new GameObject("Walls").transform;

            _spacesParent.SetParent(transform);
            _roomsParent.SetParent(transform);
            _hallwaysParent.SetParent(transform);
            _wallParent.SetParent(transform);

            _listOfRooms.Clear();
            _listOfHallways.Clear();
            _listOfHallwaysObjects.Clear();
        }

        public void GenerateLevelWithSeed()
        {
            if (_useRandomSeed)
            {
                seed = 0;
            }
            GenerateLevel();

            DataPersistenceManager.Instance.GameData.currentDungeonSeed = seed;
            DataPersistenceManager.Instance.GameData.useRandomSeed = _useRandomSeed;
        }

        private void OnApplicationQuit()
        {
            SaveValues();
        }

        private void SaveValues()
        {
            DataPersistenceManager.Instance.GameData.playerPosition = new Vector3(_currentEntryRoom.CenterPoint.x, 1, _currentEntryRoom.CenterPoint.y + 2);
            if (characterController != null && characterController.IsDead)
            {
                Debug.LogWarning("Game not saved — player is dead.");
                return;
            }

            enemySpawner.SaveEnemyPositions();

            // Save player state
            DataPersistenceManager.Instance.GameData.playerCurrentHealth = characterController.curHp;
            DataPersistenceManager.Instance.GameData.playerCurrentXp = characterController.LevelUpSystem.CurXp;
            DataPersistenceManager.Instance.GameData.playerLevel = characterController.LevelUpSystem.currentLevel;

            DataPersistenceManager.Instance.SaveGame();
            DataPersistenceManager.Instance.GameData.currentDungeonSeed = seed;
            DataPersistenceManager.Instance.GameData.useRandomSeed = false;
            DataPersistenceManager.Instance.GameData.dungeonLevel = _level;
            DataPersistenceManager.Instance.GameData.playerPosition = characterObject.transform.position;
        }
    }
}
