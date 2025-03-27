using System.Collections.Generic;
using UnityEngine;


public class WallTile
{
    public Vector2Int Position { get; private set; }

    public WallTile(Vector2Int position)
    {
        Position = position;
    }
}
