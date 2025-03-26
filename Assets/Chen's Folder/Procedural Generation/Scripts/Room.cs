using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int bottomLeftRoomCorner;
    public Vector2Int topRightRoomCorner;

    public Vector2Int bottomRightRoomCorner;
    public Vector2Int topLeftRoomCorner;

    private Vector2 centerPoint;

    private List<Room> listOfRoomsToConnect;
    public List<Room> ListOfRoomsToConnect => listOfRoomsToConnect;
    public int amountOfRoomsToConnect { get; private set; }

    public int Width => (topRightRoomCorner.x - bottomLeftRoomCorner.x);
    public int Length => (topRightRoomCorner.y - bottomLeftRoomCorner.y);

    public Vector2 CenterPoint => centerPoint;

    public bool CanAddMoreConnections => listOfRoomsToConnect.Count < amountOfRoomsToConnect;

    public Room(Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int amountOfRooms)
    {
        bottomLeftRoomCorner = bottomLeftCorner;
        topRightRoomCorner = topRightCorner;

        bottomRightRoomCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
        topLeftRoomCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);

        amountOfRoomsToConnect = amountOfRooms;
        listOfRoomsToConnect = new List<Room>();

        centerPoint = new Vector2(bottomLeftRoomCorner.x + Width / 2f, bottomLeftRoomCorner.y + Length / 2f);
    }

    public bool IsConnectedToGivenRoom(Room room)
    {
        return room != this &&
               !listOfRoomsToConnect.Contains(room) &&
               listOfRoomsToConnect.Count < amountOfRoomsToConnect;
    }

    public void AddRoomToList(Room room)
    {
        listOfRoomsToConnect.Add(room);
    }
}
