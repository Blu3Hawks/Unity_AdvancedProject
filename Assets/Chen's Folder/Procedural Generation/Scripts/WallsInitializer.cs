using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WallsInitializer
{
    private List<Vector2> listOfWallsForRooms = new List<Vector2>();
    private List<Vector2> listOfWallsForHallways = new List<Vector2>();

    public List<Vector2> ListOfWallsForRooms { get { return listOfWallsForRooms; } }
    public List<Vector2> ListOfWallsForHallways { get { return listOfWallsForHallways; } }

    public void SetupWallsForRoom(Room room, float offset)
    {
        listOfWallsForRooms.Clear();

        Vector2 wallTopLeftCorner = new Vector2(room.topLeftRoomCorner.x - offset, room.topLeftRoomCorner.y + offset);
        Vector2 wallTopRightCorner = new Vector2(room.topRightRoomCorner.x + offset, room.topRightRoomCorner.y + offset);
        Vector2 wallBottomLeftCorner = new Vector2(room.bottomLeftRoomCorner.x - offset, room.bottomLeftRoomCorner.y - offset);
        Vector2 wallBottomRightCorner = new Vector2(room.bottomRightRoomCorner.x + offset, room.bottomRightRoomCorner.y - offset);

        listOfWallsForRooms.Add(wallTopLeftCorner);
        listOfWallsForRooms.Add(wallTopRightCorner);
        listOfWallsForRooms.Add(wallBottomLeftCorner);
        listOfWallsForRooms.Add(wallBottomRightCorner);

        // Bottom wall
        AddAllPositionsBetweenCorners(wallBottomLeftCorner, wallBottomRightCorner, listOfWallsForRooms);
        // Left wall
        AddAllPositionsBetweenCorners(wallBottomLeftCorner, wallTopLeftCorner, listOfWallsForRooms);
        // Top wall
        AddAllPositionsBetweenCorners(wallTopLeftCorner, wallTopRightCorner, listOfWallsForRooms);
        // Right wall
        AddAllPositionsBetweenCorners(wallBottomRightCorner, wallTopRightCorner, listOfWallsForRooms);
    }

    public void SetupWallsForHallway(Hallway hallway, float offset)
    {
        listOfWallsForHallways.Clear();

        Vector2 wallTopLeftCorner = new Vector2(hallway.topLeftHallwayCorner.x - offset, hallway.topLeftHallwayCorner.y + offset);
        Vector2 wallTopRightCorner = new Vector2(hallway.topRightHallwayCorner.x + offset, hallway.topRightHallwayCorner.y + offset);
        Vector2 wallBottomLeftCorner = new Vector2(hallway.bottomLeftHallwayCorner.x - offset, hallway.bottomLeftHallwayCorner.y - offset);
        Vector2 wallBottomRightCorner = new Vector2(hallway.bottomRightHallwayCorner.x + offset, hallway.bottomRightHallwayCorner.y - offset);

        listOfWallsForHallways.Add(wallTopLeftCorner);
        listOfWallsForHallways.Add(wallTopRightCorner);
        listOfWallsForHallways.Add(wallBottomLeftCorner);
        listOfWallsForHallways.Add(wallBottomRightCorner);

        // Bottom wall
        AddAllPositionsBetweenCorners(wallBottomLeftCorner, wallBottomRightCorner, listOfWallsForHallways);
        // Left wall
        AddAllPositionsBetweenCorners(wallBottomLeftCorner, wallTopLeftCorner, listOfWallsForHallways);
        // Top wall
        AddAllPositionsBetweenCorners(wallTopLeftCorner, wallTopRightCorner, listOfWallsForHallways);
        // Right wall
        AddAllPositionsBetweenCorners(wallBottomRightCorner, wallTopRightCorner, listOfWallsForHallways);
    }

    // Add all positions between two corners
    private void AddAllPositionsBetweenCorners(Vector2 cornerA, Vector2 cornerB, List<Vector2> listOfWalls)
    {
        Vector2 newWallPointToAdd = cornerA;
        listOfWalls.Add(newWallPointToAdd);

        // Check if we are moving horizontally or vertically
        if (cornerA.x != cornerB.x)
        {
            // Move horizontally
            while (newWallPointToAdd.x != cornerB.x)
            {
                newWallPointToAdd.x += Mathf.Sign(cornerB.x - cornerA.x);
                listOfWalls.Add(newWallPointToAdd);
            }
        }
        else if (cornerA.y != cornerB.y)
        {
            // Move vertically
            while (newWallPointToAdd.y != cornerB.y)
            {
                newWallPointToAdd.y += Mathf.Sign(cornerB.y - cornerA.y);
                listOfWalls.Add(newWallPointToAdd);
            }
        }
    }
}
