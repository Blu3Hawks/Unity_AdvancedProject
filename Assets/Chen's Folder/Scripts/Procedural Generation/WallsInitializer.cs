using System.Collections.Generic;
using UnityEngine;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class WallsInitializer
    {
        private List<Vector2> _listOfWallsForRooms = new List<Vector2>();
        private List<Vector2> _listOfWallsForHallways = new List<Vector2>();

        public List<Vector2> ListOfWallsForRooms => _listOfWallsForRooms;
        public List<Vector2> ListOfWallsForHallways => _listOfWallsForHallways;

        public void SetupWallsForRoom(Room room, float offset)
        {
            _listOfWallsForRooms.Clear();
            AddAllPositionsBetweenRoomCorners(room.BottomLeftRoomCorner, room.BottomRightRoomCorner, offset);
            AddAllPositionsBetweenRoomCorners(room.BottomRightRoomCorner, room.TopRightRoomCorner, offset);
            AddAllPositionsBetweenRoomCorners(room.TopRightRoomCorner, room.TopLeftRoomCorner, offset);
            AddAllPositionsBetweenRoomCorners(room.TopLeftRoomCorner, room.BottomLeftRoomCorner, offset);

            RemoveOverlappingWalls(_listOfWallsForRooms, new List<Room> { room }, new List<Hallway>());
        }

        public void SetupWallsForHallway(GameObject hallwayObject, float offset, List<Room> rooms, List<Hallway> hallways)
        {
            _listOfWallsForHallways.Clear();

            Vector3 hallwayPosition = hallwayObject.transform.position;
            float xScale = hallwayObject.transform.localScale.x;
            float zScale = hallwayObject.transform.localScale.z;
            float xLength = xScale * 10;
            float zLength = zScale * 10;

            Vector2 topRightHallwayCorner = new Vector2(hallwayPosition.x + xLength / 2, hallwayPosition.z + zLength / 2);
            Vector2 bottomRightHallwayCorner = new Vector2(hallwayPosition.x + xLength / 2, hallwayPosition.z - zLength / 2);
            Vector2 topLeftHallwayCorner = new Vector2(hallwayPosition.x - xLength / 2, hallwayPosition.z + zLength / 2);
            Vector2 bottomLeftHallwayCorner = new Vector2(hallwayPosition.x - xLength / 2, hallwayPosition.z - zLength / 2);

            AddAllPositionsBetweenHallwayCorners(bottomLeftHallwayCorner, bottomRightHallwayCorner, offset);
            AddAllPositionsBetweenHallwayCorners(bottomRightHallwayCorner, topRightHallwayCorner, offset);
            AddAllPositionsBetweenHallwayCorners(topRightHallwayCorner, topLeftHallwayCorner, offset);
            AddAllPositionsBetweenHallwayCorners(topLeftHallwayCorner, bottomLeftHallwayCorner, offset);

            RemoveOverlappingWalls(_listOfWallsForHallways, rooms, hallways);
        }

        private void AddAllPositionsBetweenRoomCorners(Vector2 cornerA, Vector2 cornerB, float offset)
        {
            Vector2 direction = (cornerB - cornerA).normalized;
            float distance = Vector2.Distance(cornerA, cornerB);
            int steps = Mathf.CeilToInt(distance / offset);

            for (int i = 0; i <= steps; i++)
            {
                Vector2 position = cornerA + direction * (i * offset);
                _listOfWallsForRooms.Add(position);
            }
        }

        private void AddAllPositionsBetweenHallwayCorners(Vector2 cornerA, Vector2 cornerB, float offset)
        {
            Vector2 direction = (cornerB - cornerA).normalized;
            float distance = Vector2.Distance(cornerA, cornerB);
            int steps = Mathf.CeilToInt(distance / offset);

            for (int i = 0; i <= steps; i++)
            {
                Vector2 position = cornerA + direction * (i * offset);
                _listOfWallsForHallways.Add(position);
            }
        }

        private void RemoveOverlappingWalls(List<Vector2> wallPositions, List<Room> rooms, List<Hallway> hallways)
        {
            List<Vector2> positionsToRemove = new List<Vector2>();
            float minDistance = 0.5f;
        
            for (int i = 0; i < wallPositions.Count; i++)
            {
                for (int j = i + 1; j < wallPositions.Count; j++)
                {
                    if (Vector2.Distance(wallPositions[i], wallPositions[j]) < minDistance)
                    {
                        positionsToRemove.Add(wallPositions[j]);
                    }
                }

                foreach (Room room in rooms)
                {
                    if (IsPositionInsideRoom(wallPositions[i], room))
                    {
                        positionsToRemove.Add(wallPositions[i]);
                        break;
                    }
                }

                foreach (Hallway hallway in hallways)
                {
                    if (IsPositionInsideHallway(wallPositions[i], hallway))
                    {
                        positionsToRemove.Add(wallPositions[i]);
                        break;
                    }
                }
            }

            foreach (Vector2 position in positionsToRemove)
            {
                wallPositions.Remove(position);
            }
        }

        private bool IsPositionInsideRoom(Vector2 position, Room room)
        {
            return position.x > room.BottomLeftRoomCorner.x && position.x < room.TopRightRoomCorner.x &&
                   position.y > room.BottomLeftRoomCorner.y && position.y < room.TopRightRoomCorner.y;
        }

        private bool IsPositionInsideHallway(Vector2 position, Hallway hallway)
        {
            return position.x > hallway.BottomLeftHallwayCorner.x && position.x < hallway.TopRightHallwayCorner.x &&
                   position.y > hallway.BottomLeftHallwayCorner.y && position.y < hallway.TopRightHallwayCorner.y;
        }

        public void RemoveOverlappingWallsBetweenHallways(List<Hallway> hallways, float minDistance = 0.5f)
        {
            List<Hallway> hallwaysToRemove = new List<Hallway>();

            for (int i = 0; i < hallways.Count; i++)
            {
                for (int j = i + 1; j < hallways.Count; j++)
                {
                    if (AreHallwaysOverlapping(hallways[i], hallways[j], minDistance))
                    {
                        hallwaysToRemove.Add(hallways[j]);
                    }
                }
            }

            foreach (Hallway hallway in hallwaysToRemove)
            {
                hallways.Remove(hallway);
            }
        }

        private bool AreHallwaysOverlapping(Hallway hallwayA, Hallway hallwayB, float minDistance)
        {
            return IsPositionInsideHallway(hallwayA.BottomLeftHallwayCorner, hallwayB) ||
                   IsPositionInsideHallway(hallwayA.TopRightHallwayCorner, hallwayB) ||
                   IsPositionInsideHallway(hallwayA.BottomRightHallwayCorner, hallwayB) ||
                   IsPositionInsideHallway(hallwayA.TopLeftHallwayCorner, hallwayB) ||
                   Vector2.Distance(hallwayA.CenterPoint, hallwayB.CenterPoint) < minDistance;
        }


        public void RemoveAllOverlappingWalls(List<Room> listOfRooms, List<GameObject> listOfHallways)
        {
            List<Vector2> roomWallsToRemove = new List<Vector2>();
            List<Vector2> hallwayWallsToRemove = new List<Vector2>();

            // Remove room walls that fall inside room bounds (duplicate/internal)
            foreach (Room room in listOfRooms)
            {
                foreach (GameObject hallway in listOfHallways)
                {
                    Vector3 hallwayPos = hallway.transform.position;
                    Vector3 hallwayScale = hallway.transform.localScale;

                    Vector2 hallwayBL = new Vector2(hallwayPos.x - hallwayScale.x * 5f, hallwayPos.z - hallwayScale.z * 5f);
                    Vector2 hallwayTR = new Vector2(hallwayPos.x + hallwayScale.x * 5f, hallwayPos.z + hallwayScale.z * 5f);

                    foreach (Vector2 wallPos in _listOfWallsForRooms)
                    {
                        if (wallPos.x >= hallwayBL.x && wallPos.x <= hallwayTR.x &&
                            wallPos.y >= hallwayBL.y && wallPos.y <= hallwayTR.y)
                        {
                            roomWallsToRemove.Add(wallPos);
                        }
                    }
                }
            }

            // Remove hallway walls that fall inside any room
            foreach (GameObject hallway in listOfHallways)
            {
                Vector3 hallwayPos = hallway.transform.position;
                Vector3 hallwayScale = hallway.transform.localScale;

                Vector2 bottomLeft = new Vector2(hallwayPos.x - hallwayScale.x * 5f, hallwayPos.z - hallwayScale.z * 5f);
                Vector2 topRight = new Vector2(hallwayPos.x + hallwayScale.x * 5f, hallwayPos.z + hallwayScale.z * 5f);

                foreach (Vector2 wallPosition in _listOfWallsForHallways)
                {
                    foreach (Room room in listOfRooms)
                    {
                        if (wallPosition.x > room.BottomLeftRoomCorner.x && wallPosition.x < room.TopRightRoomCorner.x &&
                            wallPosition.y > room.BottomLeftRoomCorner.y && wallPosition.y < room.TopRightRoomCorner.y)
                        {
                            hallwayWallsToRemove.Add(wallPosition);
                        }
                    }

                    // Optionally: remove hallway walls that are inside the hallway box itself
                    if (wallPosition.x > bottomLeft.x && wallPosition.x < topRight.x &&
                        wallPosition.y > bottomLeft.y && wallPosition.y < topRight.y)
                    {
                        hallwayWallsToRemove.Add(wallPosition);
                    }
                }
            }

            // Remove duplicates from final lists
            foreach (Vector2 pos in roomWallsToRemove)
                _listOfWallsForRooms.Remove(pos);

            foreach (Vector2 pos in hallwayWallsToRemove)
                _listOfWallsForHallways.Remove(pos);
        }

    }
}
