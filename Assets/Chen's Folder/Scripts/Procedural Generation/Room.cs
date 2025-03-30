using System.Collections.Generic;
using UnityEngine;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class Room
    {
        public Vector2Int BottomLeftRoomCorner;
        public Vector2Int TopRightRoomCorner;

        public Vector2Int BottomRightRoomCorner;
        public Vector2Int TopLeftRoomCorner;

        private Vector2 _centerPoint;

        private List<Room> _listOfRoomsToConnect;
        public List<Room> ListOfRoomsToConnect => _listOfRoomsToConnect;
        public int AmountOfRoomsToConnect { get; private set; }

        public int Width => (TopRightRoomCorner.x - BottomLeftRoomCorner.x);
        public int Length => (TopRightRoomCorner.y - BottomLeftRoomCorner.y);

        public Vector2 CenterPoint => _centerPoint;

        public bool CanAddMoreConnections => _listOfRoomsToConnect.Count < AmountOfRoomsToConnect;

        public Room(Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int amountOfRooms)
        {
            BottomLeftRoomCorner = bottomLeftCorner;
            TopRightRoomCorner = topRightCorner;

            BottomRightRoomCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
            TopLeftRoomCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);

            AmountOfRoomsToConnect = amountOfRooms;
            _listOfRoomsToConnect = new List<Room>();

            _centerPoint = new Vector2(BottomLeftRoomCorner.x + Width / 2f, BottomLeftRoomCorner.y + Length / 2f);
        }

        public bool IsConnectedToGivenRoom(Room room)
        {
            return room != this &&
                   !_listOfRoomsToConnect.Contains(room) &&
                   _listOfRoomsToConnect.Count < AmountOfRoomsToConnect;
        }

        public void AddRoomToList(Room room)
        {
            _listOfRoomsToConnect.Add(room);
        }
    }
}
