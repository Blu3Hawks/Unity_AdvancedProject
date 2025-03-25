using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevelGenerator : MonoBehaviour
{
    [Header("Level Values")]
    [Header("Level Width")]
    // min and max level width with a local variable of the actual width
    [SerializeField] private int minLevelWidth;
    [SerializeField] private int maxLevelWidth;
    private int levelWidth;
    [Header("Level Length")]
    // min and max level length with a local variable of the actual length
    [SerializeField] private int minLevelLength;
    [SerializeField] private int maxLevelLength;
    private int levelLength;
    [Header("Level Iterations")]
    // min and max iterations - how many divides will be in the total space given with a local variable of the actual iterations
    [SerializeField] private int minIterations;
    [SerializeField] private int maxIterations;
    private int levelIterations;

    [Header("Room Values")]
    //track the amount of rooms that we have and also their min and max values for their size
    [Header("Room Width")]
    [SerializeField] private int minRoomWidth;
    [SerializeField] private int maxRoomWidth;
    [Header("Room Length")]
    [SerializeField] private int minRoomLength;
    [SerializeField] private int maxRoomLength;
    [Header("List of Room")]

    [SerializeField] private GameObject planePrefab;
    [SerializeField] private GameObject roomPlanePrefab;

    SpaceDivider spaceDivider;

    private List<Room> listOfRooms = new List<Room>();

    public List<SpaceArea> spacesToDivide = new List<SpaceArea>();
    public List<SpaceArea> spacesToPrint = new List<SpaceArea>();
    public void GenerateLevel()
    {
        ClearWorld();
        GenerateRandomLevelValues();
        spaceDivider = new SpaceDivider(levelWidth, levelLength, levelIterations, maxRoomWidth, maxRoomLength);
        GenerateAllSpaces();

        Debug.Log("The width is: " + levelWidth + " and the length is: " + levelLength);

        CreatePlanesForSpaces();
        CreatePlanesForRooms();
        Debug.Log(spaceDivider.spacesToPrint.Count.ToString());
    }

    private void GenerateRandomLevelValues()
    {
        levelWidth = UnityEngine.Random.Range(minLevelWidth, maxLevelWidth);
        levelLength = UnityEngine.Random.Range(minLevelLength, maxLevelLength);
        levelIterations = UnityEngine.Random.Range(minIterations, maxIterations);
    }

    private void CreatePlanesForSpaces()
    {
        foreach (SpaceArea space in spacesToPrint)
        {
            if (planePrefab == null)
            {
                Debug.LogError("Plane prefab is not assigned!");
                return;
            }

            GameObject plane = Instantiate(planePrefab, transform);

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
            if (planePrefab == null)
            {
                Debug.LogError("Plane prefab is not assigned!");
                return;
            }

            GameObject plane = Instantiate(roomPlanePrefab, transform);
            Material material = plane.GetComponent<Material>();

            // material.color = new Color(UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255));

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

            plane.transform.position = position;
            plane.transform.localScale = scale;
        }
    }

    public void GenerateRoom(SpaceArea givenSpace)
    {
        int randomRoomWidth = UnityEngine.Random.Range(minRoomWidth, maxRoomLength);
        int randomRoomLength = UnityEngine.Random.Range(minRoomLength, maxRoomLength);

        Vector2Int newBottomLeftCorner = new Vector2Int(
            UnityEngine.Random.Range( //this is the random X value of the bottom left corner
            givenSpace.bottomLeftSpaceCorner.x + 1, givenSpace.bottomRightSpaceCorner.x - 1 - randomRoomWidth),
            UnityEngine.Random.Range( //this is the random Y value of the bottom left corner
            givenSpace.bottomLeftSpaceCorner.y + 1, givenSpace.topLeftSpaceCorner.y - 1 - randomRoomLength)
            );
        //now we calculate the top right corner

        Vector2Int newTopRightCorner = new Vector2Int(
            newBottomLeftCorner.x + randomRoomWidth,
            newBottomLeftCorner.y + randomRoomLength);

        Room newRoom = new Room(newBottomLeftCorner, newTopRightCorner);
        listOfRooms.Add(newRoom);
    }

    public void GenerateAllSpaces()
    {
        // getting the firt space here - then dividing it for the amount of iterations, each time creating new spaces, that are on a list - after dividing remove them from the list. each time
        // check a random space to divide until run out of divisions/ iterations or running out of space to divide.
        SpaceArea firstSpace = new SpaceArea(new Vector2Int(0, 0), new Vector2Int(levelWidth, levelLength));
        spacesToDivide.Add(firstSpace);
        int currentIteration = 0;

        while (currentIteration < levelIterations && spacesToDivide.Count > 0)
        {
            int randomSpaceToDivide = UnityEngine.Random.Range(0, spacesToDivide.Count);
            SpaceArea spaceToCheck = spacesToDivide[randomSpaceToDivide];

            if (CheckForSpaceDivisionXAxis(spaceToCheck) && CheckForSpaceDivisionYAxis(spaceToCheck) && spacesToDivide.Contains(spaceToCheck))
            {
                //if both options are available then I want it to be randomized.
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    DivideSpaceXAxis(spaceToCheck);
                }
                else
                {
                    DivideSpaceYAxis(spaceToCheck);
                }
                currentIteration++;
            }
            else if (CheckForSpaceDivisionXAxis(spaceToCheck))
            {
                DivideSpaceXAxis(spaceToCheck);
                currentIteration++;
            }
            else if (CheckForSpaceDivisionYAxis(spaceToCheck))
            {
                DivideSpaceYAxis(spaceToCheck);
                currentIteration++;
            }
            else
            {
                spacesToDivide.Remove(spaceToCheck);
                spacesToPrint.Add(spaceToCheck);
                continue;
            }
        }


        //now after the while loop is broken we can add the remaining spaces to the list that we print out - our final spaces
        foreach (SpaceArea space in spacesToDivide)
        {
            spacesToPrint.Add(space);
        }

        foreach (SpaceArea space in spacesToPrint)
        {
            GenerateRoom(space);
        }
    }


    private void DivideSpaceXAxis(SpaceArea spaceToCheck)
    {
        //first we remove it from the list
        spacesToDivide.Remove(spaceToCheck);
        // take the total width - between the min x value + (max room length + 1) and the max x value - (max room length +1)

        int randomXValue = UnityEngine.Random.Range(
            spaceToCheck.bottomLeftSpaceCorner.x + (maxRoomWidth + 1),
            spaceToCheck.bottomRightSpaceCorner.x - (maxRoomWidth + 1)
            );

        if (randomXValue <= spaceToCheck.bottomLeftSpaceCorner.x || randomXValue >= spaceToCheck.topRightSpaceCorner.x)
        {
            Debug.LogWarning("Invalid X-axis division point. Skipping this space.");
            return;
        }
        //now we create two new spaces - the first one, that will be the left part of the divided space, will be having the bottom left and top left corners,
        //combined with two new points - the top right would be the top left + the newXValue and the bottom right will be the bottom left + newXValue

        SpaceArea newSpaceLeftSide = new SpaceArea(
            spaceToCheck.bottomLeftSpaceCorner,
            new Vector2Int(randomXValue,
            spaceToCheck.topRightSpaceCorner.y)
            );
        //next we will take the other side and make a space out of it - so we will take the middle values, which are the previous space that we have created - we will take its right points
        //as our left points for the new space, and the right points would be the given space right points. Watch the files to get a better understanding for the divisions.

        //we can use the new space on the left - and take its bottom right point, and the given space's top right point to create this new space. That totally works.

        SpaceArea newSpaceRightSide = new SpaceArea(
            new Vector2Int(randomXValue, spaceToCheck.bottomLeftSpaceCorner.y),
            spaceToCheck.topRightSpaceCorner
            );
        //now we will add these two spaces to the list

        spacesToDivide.Add(newSpaceLeftSide);
        spacesToDivide.Add(newSpaceRightSide);
    }

    private void DivideSpaceYAxis(SpaceArea spaceToCheck)
    {
        spacesToDivide.Remove(spaceToCheck);
        //take the total length and randomise between the min y value + max room length + 1 and the max y value - (max room length +1)

        int randomYValue = UnityEngine.Random.Range(
            spaceToCheck.bottomLeftSpaceCorner.y + (maxRoomLength + 1),
            spaceToCheck.topLeftSpaceCorner.y - (maxRoomWidth + 1)
            );

        if (randomYValue <= spaceToCheck.bottomLeftSpaceCorner.y || randomYValue >= spaceToCheck.topRightSpaceCorner.y)
        {
            Debug.LogWarning("Invalid Y-axis division point. Skipping this space.");
            return;
        }

        //now we can create two new spaced - the upper space and bottom. first we make the upper one, so we will make the upper one first
        //it will have the top left and top right corners and then the new bottom left and bottom right points would be
        //the original bottom left and right corners + the new Y Value given.

        SpaceArea newSpaceDownSide = new SpaceArea(
            spaceToCheck.bottomLeftSpaceCorner,
            new Vector2Int(spaceToCheck.bottomRightSpaceCorner.x, randomYValue)
            );
        //now we do the same with the new space at the bottom

        SpaceArea newSpaceUpSide = new SpaceArea(
            newSpaceDownSide.topLeftSpaceCorner,
            spaceToCheck.topRightSpaceCorner
            );
        //now we will add these two spaces to the list

        spacesToDivide.Add(newSpaceUpSide);
        spacesToDivide.Add(newSpaceDownSide);
    }


    private bool CheckForSpaceDivisionXAxis(SpaceArea spaceToCheck)
    {
        return spaceToCheck.Width > (maxRoomWidth + 1) * 2;
    }

    private bool CheckForSpaceDivisionYAxis(SpaceArea spaceToCheck)
    {
        return spaceToCheck.Length > (maxRoomLength + 1) * 2;
    }

    private void ClearWorld()
    {
        Console.Clear();
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
        spacesToDivide.Clear();
        spacesToPrint.Clear();
        listOfRooms.Clear();
    }
}