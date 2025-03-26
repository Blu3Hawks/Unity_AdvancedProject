using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector
{
    private List<Room> rooms;
    public RoomConnector(List<Room> allRooms)
    {
        rooms = allRooms;
        ConnectRoomsWithMinimumGuarantee();
    }

    public void ConnectRoomsWithMinimumGuarantee()
    {
        //hashset for any room that we have visited, also adding a priority Queue for that matter
        HashSet<Room> visited = new HashSet<Room>();
        PriorityQueue<Room, float> priorityQueue = new PriorityQueue<Room, float>();
        //we start from first room in the list and add it up
        Room start = rooms[0];
        visited.Add(start);
        //adding up the neighbor room
        EnqueueNeighbors(start, priorityQueue, visited);

        //now that we have all of the priority queue and neighbors set up, we can loop through them
        while (priorityQueue.Count > 0)
        {
            //we will first dequeu them from our queue so we dont loop them again by accident
            Room currentRoom = priorityQueue.Dequeue();
            //if we didn't visit it, then we should
            if (!visited.Contains(currentRoom))
            {
                //what's the closest room that we didn';t visit ? 
                Room closestUnvisited = FindClosestRoom(currentRoom, room => !visited.Contains(room));
                if (closestUnvisited != null)
                {
                    currentRoom.AddRoomToList(closestUnvisited);
                    closestUnvisited.AddRoomToList(currentRoom);
                    visited.Add(currentRoom);
                    EnqueueNeighbors(currentRoom, priorityQueue, visited);
                }
            }
        }

        //here we are just clarifying that we have every room passed and make sure that if we missed, then we will add it as well
        foreach (Room room in rooms)
        {
            if (!visited.Contains(room))
            {
                Room closestVisited = FindClosestRoom(room, visited.Contains);
                if (closestVisited != null)
                {
                    room.AddRoomToList(closestVisited);
                    closestVisited.AddRoomToList(room);
                    visited.Add(room);
                }
            }
        }

        //here we sort it all by the distance, and check if they are the closest rooms for each of them
        //basically saying - can we connect rooms to it? does it have already a connection to the room we want to check?
        //well, let's make them connect now, so they will have a direct connection
        foreach (Room room in rooms)
        {
            List<Room> sortedByDistance = new List<Room>(rooms);
            sortedByDistance.Sort((a, b) =>
                Vector2.Distance(room.CenterPoint, a.CenterPoint)
                .CompareTo(Vector2.Distance(room.CenterPoint, b.CenterPoint)));

            foreach (Room possibleRoomConnection in sortedByDistance)
            {
                if (room.IsConnectedToGivenRoom(possibleRoomConnection) && possibleRoomConnection.IsConnectedToGivenRoom(room))
                {
                    room.AddRoomToList(possibleRoomConnection);
                    possibleRoomConnection.AddRoomToList(room);

                    if (!room.CanAddMoreConnections)
                        break;
                }
            }
        }
    }

    //Enqueue the neighbors properly, so they will always find which room is a neiugbor of who
    private void EnqueueNeighbors(Room room, PriorityQueue<Room, float> priorityQueue, HashSet<Room> visitedRoom)
    {
        //so for each room that we have, we check for the specific room if they are neighbors or not
        foreach (Room neighborRoom in rooms)
        {
            if (!visitedRoom.Contains(neighborRoom))
            {
                //if we didn't classify them as neighbors then it will add them to the priority's queue
                float distance = Vector2.Distance(room.CenterPoint, neighborRoom.CenterPoint);
                priorityQueue.Enqueue(neighborRoom, distance);
            }
        }
    }

    //find the closest room to the given room, and calculate its distance
    private Room FindClosestRoom(Room roomToCheck, System.Predicate<Room> condition)
    {
        Room closestRoom = null;
        float minDist = float.MaxValue;

        foreach (Room candidate in rooms)
        {
            if (candidate == roomToCheck || !condition(candidate)) continue;

            float dist = Vector2.Distance(roomToCheck.CenterPoint, candidate.CenterPoint);
            if (dist < minDist)
            {
                minDist = dist;
                closestRoom = candidate;
            }
        }

        return closestRoom;
    }

    public Vector2 GetClosestSidePoint(Room room, Vector2 point)
    {
        Vector2 closestPoint = room.CenterPoint;
        float minDist = float.MaxValue;

        //get the sides here
        Vector2[] sides = new Vector2[]
        {
            new Vector2(room.bottomLeftRoomCorner.x, room.CenterPoint.y), //left
            new Vector2(room.topRightRoomCorner.x, room.CenterPoint.y), //right
            new Vector2(room.CenterPoint.x, room.bottomLeftRoomCorner.y), //down
            new Vector2(room.CenterPoint.x, room.topRightRoomCorner.y) //upp
        };

        //check the shortest distance
        foreach (Vector2 side in sides)
        {
            float dist = Vector2.Distance(point, side);
            if (dist < minDist)
            {
                minDist = dist;
                closestPoint = side;
            }
        }

        return closestPoint;
    }

}
