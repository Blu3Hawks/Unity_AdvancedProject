using UnityEngine;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class Hallway
    {
        public Vector2Int BottomLeftHallwayCorner { get; private set; }
        public Vector2Int TopRightHallwayCorner { get; private set; }
        public Vector2Int BottomRightHallwayCorner { get; private set; }
        public Vector2Int TopLeftHallwayCorner { get; private set; }
        public Vector2 CenterPoint { get; private set; }
        public int Width => TopRightHallwayCorner.x - BottomLeftHallwayCorner.x;
        public int Length => TopRightHallwayCorner.y - BottomLeftHallwayCorner.y;

        public Hallway(Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
        {
            BottomLeftHallwayCorner = bottomLeftCorner;
            TopRightHallwayCorner = topRightCorner;
            BottomRightHallwayCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
            TopLeftHallwayCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);
            CenterPoint = (bottomLeftCorner + topRightCorner) / 2;
        }
    }
}
