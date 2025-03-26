using System.Collections.Generic;
using UnityEngine;

public class Hallway
{
    public Vector2Int bottomLeftHallwayCorner;
    public Vector2Int topRightHallwayCorner;

    public Vector2Int bottomRightHallwayCorner;
    public Vector2Int topLeftHallwayCorner;

    private Vector2 centerPoint;

    private List<Hallway> listOfHallwaysToConnect;
    public List<Hallway> ListOfHallwaysToConnect => listOfHallwaysToConnect;
    public int amountOfHallwaysToConnect { get; private set; }

    public int Width => (topRightHallwayCorner.x - bottomLeftHallwayCorner.x);
    public int Length => (topRightHallwayCorner.y - bottomLeftHallwayCorner.y);

    public Vector2 CenterPoint => centerPoint;

    public bool CanAddMoreConnections => listOfHallwaysToConnect.Count < amountOfHallwaysToConnect;

    public Hallway(Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int amountOfHallways)
    {
        bottomLeftHallwayCorner = bottomLeftCorner;
        topRightHallwayCorner = topRightCorner;

        bottomRightHallwayCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
        topLeftHallwayCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);

        amountOfHallwaysToConnect = amountOfHallways;
        listOfHallwaysToConnect = new List<Hallway>();

        centerPoint = new Vector2(bottomLeftHallwayCorner.x + Width / 2f, bottomLeftHallwayCorner.y + Length / 2f);
    }

}
