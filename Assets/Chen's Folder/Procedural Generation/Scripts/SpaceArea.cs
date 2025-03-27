using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceArea
{
    public Vector2Int bottomLeftSpaceCorner;
    public Vector2Int topRightSpaceCorner;

    public Vector2Int bottomRightSpaceCorner;
    public Vector2Int topLeftSpaceCorner;

    public int Width { get => (topRightSpaceCorner.x - bottomLeftSpaceCorner.x); }
    public int Length { get => (topRightSpaceCorner.y - bottomLeftSpaceCorner.y); }


    // a public constructor for the points of the space - a square
    public SpaceArea(Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
    {
        bottomLeftSpaceCorner = bottomLeftCorner;
        topRightSpaceCorner = topRightCorner;

        bottomRightSpaceCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
        topLeftSpaceCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);
    }
}
