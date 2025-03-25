using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int bottomLeftRoomCorner;
    public Vector2Int topRightRoomCorner;

    public Vector2Int bottomRightRoomCorner;
    public Vector2Int topLeftRoomCorner;

    public Vector2 centerPoint;

    public int Width { get => (topRightRoomCorner.x - bottomLeftRoomCorner.x); }
    public int Length { get => (topRightRoomCorner.y - bottomLeftRoomCorner.y); }

    public Vector2 CenterPoint { get => centerPoint; set => centerPoint = new Vector2Int(bottomLeftRoomCorner.x + Width / 2, bottomLeftRoomCorner.y + Length / 2); }
    public Room(Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
    {
        bottomLeftRoomCorner = bottomLeftCorner;
        topRightRoomCorner = topRightCorner;

        bottomRightRoomCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
        topLeftRoomCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);
    }
}
