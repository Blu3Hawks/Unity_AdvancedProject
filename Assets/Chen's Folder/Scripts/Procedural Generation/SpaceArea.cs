using UnityEngine;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class SpaceArea
    {
        public Vector2Int BottomLeftSpaceCorner;
        public Vector2Int TopRightSpaceCorner;

        public Vector2Int BottomRightSpaceCorner;
        public Vector2Int TopLeftSpaceCorner;

        public int Width { get => (TopRightSpaceCorner.x - BottomLeftSpaceCorner.x); }
        public int Length { get => (TopRightSpaceCorner.y - BottomLeftSpaceCorner.y); }


        // a public constructor for the points of the space - a square
        public SpaceArea(Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
        {
            BottomLeftSpaceCorner = bottomLeftCorner;
            TopRightSpaceCorner = topRightCorner;

            BottomRightSpaceCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
            TopLeftSpaceCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);
        }
    }
}
