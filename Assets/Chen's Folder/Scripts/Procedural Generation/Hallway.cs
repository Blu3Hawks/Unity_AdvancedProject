using System.Collections.Generic;
using UnityEngine;

public class Hallway
{
    public Vector2Int bottomLeftHallwayCorner { get; private set; }
    public Vector2Int topRightHallwayCorner { get; private set; }
    public Vector2Int bottomRightHallwayCorner { get; private set; }
    public Vector2Int topLeftHallwayCorner { get; private set; }
    public Vector2 CenterPoint { get; private set; }
    public int Width => topRightHallwayCorner.x - bottomLeftHallwayCorner.x;
    public int Length => topRightHallwayCorner.y - bottomLeftHallwayCorner.y;

    public Hallway(Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
    {
        bottomLeftHallwayCorner = bottomLeftCorner;
        topRightHallwayCorner = topRightCorner;
        bottomRightHallwayCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
        topLeftHallwayCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);
        CenterPoint = (bottomLeftCorner + topRightCorner) / 2;
    }
}
