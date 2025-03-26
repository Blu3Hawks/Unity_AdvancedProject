using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using random = UnityEngine.Random;

public class DungeonLevelGenerator : MonoBehaviour
{
    [Header("Level Values")]
    [SerializeField] private int minLevelWidth;
    [SerializeField] private int maxLevelWidth;
    private int levelWidth;

    [SerializeField] private int minLevelLength;
    [SerializeField] private int maxLevelLength;
    private int levelLength;

    [SerializeField] private int minIterations;
    [SerializeField] private int maxIterations;
    private int levelIterations;

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

    [Header("Seed")]
    [SerializeField] private int seed;
    [SerializeField] private bool useRandomSeed = true;

    private SpaceDivider spaceDivider;
    private List<Room> listOfRooms = new List<Room>();
    private List<Hallway> listOfHallways = new List<Hallway>();
    private List<GameObject> listOfHallwaysObjects = new List<GameObject>();

    private RoomConnector roomConnector;
    private WallsInitializer wallsInitializer;

    private Transform spacesParent;
    private Transform roomsParent;
    private Transform hallwaysParent;
    private Transform wallParent;

    public void GenerateLevel()
    {
        ClearWorld();
        GenerateRandomLevelValues();
        spaceDivider = new SpaceDivider(levelWidth, levelLength, levelIterations, maxRoomWidth, maxRoomLength);
        spaceDivider.GenerateAllSpaces();

        foreach (SpaceArea space in spaceDivider.SpacesToPrint)
        {
            GenerateRoom(space);
        }

        roomConnector = new RoomConnector(listOfRooms);

        CreatePlanesForSpaces();
        CreatePlanesForRooms();
        CreatePlanesForHallways(roomConnector);

        wallsInitializer = new WallsInitializer();
        foreach (Room room in listOfRooms)
        {
            wallsInitializer.SetupWallsForRoom(room, 1f);
            wallsInitializer.RemoveAllOverlappingWalls(listOfRooms, listOfHallwaysObjects);
            CreateWallsForRooms();

        }

        foreach (GameObject hallwayObject in listOfHallwaysObjects)
        {
            wallsInitializer.SetupWallsForHallway(hallwayObject, 1f, listOfRooms, listOfHallways);
            wallsInitializer.RemoveAllOverlappingWalls(listOfRooms, listOfHallwaysObjects);
            CreateWallsForHallways();
        }


        //we will check if, somehow, we have a room with no connections
        foreach (Room room in listOfRooms)
        {
            if (room.amountOfRoomsToConnect <= 0)
            {
                GenerateLevel();
            }
        }
    }

    private void GenerateRandomLevelValues()
    {
        if (useRandomSeed)
        {
            seed = random.Range(int.MinValue, int.MaxValue);
        }
        random.InitState(seed);

        levelWidth = random.Range(minLevelWidth, maxLevelWidth);
        levelLength = random.Range(minLevelLength, maxLevelLength);
        levelIterations = random.Range(minIterations, maxIterations);
    }

    private void CreateWallsForHallways()
    {
        foreach (Vector2 wallPosition in wallsInitializer.ListOfWallsForHallways)
        {
            if (wallPrefab == null)
            {
                Debug.LogError("Wall prefab is not assigned!");
                return;
            }
            Vector3 adjustedPosition = new Vector3(wallPosition.x, 1, wallPosition.y);

            GameObject wall = Instantiate(wallPrefab, wallParent);

            wall.transform.position = adjustedPosition;
        }
    }

    private void CreateWallsForRooms()
    {
        foreach (Vector2 wallPosition in wallsInitializer.ListOfWallsForRooms)
        {
            if (wallPrefab == null)
            {
                Debug.LogError("Wall prefab is not assigned!");
                return;
            }
            Vector3 adjustedPosition = new Vector3(wallPosition.x, 1, wallPosition.y);

            GameObject wall = Instantiate(wallPrefab, wallParent);

            wall.transform.position = adjustedPosition;
        }
    }

    private void CreatePlanesForSpaces()
    {
        foreach (SpaceArea space in spaceDivider.SpacesToPrint)
        {
            if (planePrefab == null)
            {
                Debug.LogError("Plane prefab is not assigned!");
                return;
            }

            GameObject plane = Instantiate(planePrefab, spacesParent);

            Vector3 position = new Vector3(
                (space.bottomLeftSpaceCorner.x + space.topRightSpaceCorner.x) / 2f,
                0,
                (space.bottomLeftSpaceCorner.y + space.topRightSpaceCorner.y) / 2f
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
        foreach (Room room in listOfRooms)
        {
            if (roomPlanePrefab == null)
            {
                Debug.LogError("Room plane prefab is not assigned!");
                return;
            }

            GameObject roomPlane = Instantiate(roomPlanePrefab, roomsParent);

            Vector3 position = new Vector3(
                (room.bottomLeftRoomCorner.x + room.topRightRoomCorner.x) / 2f,
                1,
                (room.bottomLeftRoomCorner.y + room.topRightRoomCorner.y) / 2f
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
            random.Range(givenSpace.bottomLeftSpaceCorner.x + 1, givenSpace.bottomRightSpaceCorner.x - 1 - randomRoomWidth),
            random.Range(givenSpace.bottomLeftSpaceCorner.y + 1, givenSpace.topLeftSpaceCorner.y - 1 - randomRoomLength)
        );

        Vector2Int newTopRightCorner = new Vector2Int(
            newBottomLeftCorner.x + randomRoomWidth,
            newBottomLeftCorner.y + randomRoomLength
        );

        int randomAmountOfRoomsToConnect = random.Range(1, 4);

        Room newRoom = new Room(newBottomLeftCorner, newTopRightCorner, randomAmountOfRoomsToConnect);
        listOfRooms.Add(newRoom);
    }

    private void CreatePlanesForHallways(RoomConnector roomConnector)
    {
        //have a hashset for the pairs of rooms, so every 2 rooms that are connected considered paired
        HashSet<(Room, Room)> connectedPairs = new HashSet<(Room, Room)>();

        foreach (Room room in listOfRooms)
        {
            foreach (Room connected in room.ListOfRoomsToConnect)
            {
                //make sure we don't repeat the same rooms
                if (connectedPairs.Contains((connected, room)) || connectedPairs.Contains((room, connected)))
                    continue;

                //add them to the hashset and instantiate
                connectedPairs.Add((room, connected));
                InstantiateHallwayBetweenRooms(room, connected, roomConnector);
            }
        }
    }

    private void InstantiateHallwayBetweenRooms(Room roomA, Room roomB, RoomConnector roomConnector)
    {
        //check starting and lasting rooms' center points
        Vector2 startPoint = roomConnector.GetClosestSidePoint(roomA, roomB.CenterPoint);
        Vector2 endPoint = roomConnector.GetClosestSidePoint(roomB, roomA.CenterPoint);

        //start by horizontally instantiation
        Vector2 horizontal = new Vector2(endPoint.x, startPoint.y);
        SpawnHallway(startPoint, horizontal);
        //then move vertically
        Vector2 vertical = endPoint;
        SpawnHallway(horizontal, vertical);
    }

    //here we get a start point and an end point, by getting these values,
    //we ensure that we will be moving properly horizontally first and then vertically
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

        Vector3 position = new Vector3(midPoint.x, 0.9f, midPoint.y);
        Vector3 scale;

        //check the direction that we will be facing, and scale it properly towards where we're heading
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            scale = new Vector3(length / 9.3f, 1f, hallwayWidth / 9.3f);
        }
        else
        {
            scale = new Vector3(hallwayWidth / 9.3f, 1f, length / 9.3f);
        }

        //instantiate the hallway GameObject
        GameObject hallwayObject = Instantiate(hallwayPrefab, position, Quaternion.identity, hallwaysParent);
        hallwayObject.transform.localScale = scale;
        listOfHallwaysObjects.Add(hallwayObject);

        //Create hallway and add it to the list of hallways
        Vector2Int bottomLeftCorner = Vector2Int.RoundToInt(startPoint);
        Vector2Int topRightCorner = Vector2Int.RoundToInt(endPoint);
        Hallway newHallway = new Hallway(bottomLeftCorner, topRightCorner);
        listOfHallways.Add(newHallway);
    }


    //here we are going to add a starting and ending point. So the game will choose a random room, every time, one for entry and one for exit.
    //we will have the object themselves decide what room

    private void DeclareStartingAndExitRooms()
    {
        Room entryRoom;
        Room exitRoom;

        List<Room> allRooms = listOfRooms;
        
    }


    private void ClearWorld()
    {
        if (spacesParent != null)
        {
            foreach (Transform child in spacesParent)
            {
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(spacesParent.gameObject);
        }

        if (roomsParent != null)
        {
            foreach (Transform child in roomsParent)
            {
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(roomsParent.gameObject);
        }

        if (hallwaysParent != null)
        {
            foreach (Transform child in hallwaysParent)
            {
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(hallwaysParent.gameObject);
        }

        if (wallParent != null)
        {
            foreach (Transform child in wallParent)
            {
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(wallParent.gameObject);
        }

        spacesParent = new GameObject("Spaces").transform;
        roomsParent = new GameObject("Rooms").transform;
        hallwaysParent = new GameObject("Hallways").transform;
        wallParent = new GameObject("Walls").transform;

        spacesParent.SetParent(transform);
        roomsParent.SetParent(transform);
        hallwaysParent.SetParent(transform);
        wallParent.SetParent(transform);

        listOfRooms.Clear();
        listOfHallways.Clear();
        listOfHallwaysObjects.Clear();
    }
}
