using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class SpaceDivider
    {
        private int _levelWidth;
        private int _levelLength;
        private int _maxRoomWidth;
        private int _maxRoomLength;
        private int _levelIterations;

        private List<SpaceArea> SpacesToDivide { get; set; } = new List<SpaceArea>();
        public List<SpaceArea> SpacesToPrint { get; private set; } = new List<SpaceArea>();

        public SpaceDivider(int levelWidth, int levelLength, int levelIterations, int maxRoomWidth, int maxRoomLength)
        {
            this._levelWidth = levelWidth;
            this._levelLength = levelLength;
            this._levelIterations = levelIterations;
            this._maxRoomWidth = maxRoomWidth;
            this._maxRoomLength = maxRoomLength;
        }

        public void GenerateAllSpaces()
        {
            SpaceArea firstSpace = new SpaceArea(new Vector2Int(0, 0), new Vector2Int(_levelWidth, _levelLength));
            SpacesToDivide.Add(firstSpace);
            int currentIteration = 0;

            while (currentIteration < _levelIterations && SpacesToDivide.Count > 0)
            {
                int randomSpaceToDivide = random.Range(0, SpacesToDivide.Count);
                SpaceArea spaceToCheck = SpacesToDivide[randomSpaceToDivide];

                if (CheckForSpaceDivisionXAxis(spaceToCheck) && CheckForSpaceDivisionYAxis(spaceToCheck))
                {
                    if (random.Range(0, 2) == 0)
                    {
                        DivideSpaceXAxis(spaceToCheck);
                    }
                    else
                    {
                        DivideSpaceYAxis(spaceToCheck);
                    }
                    currentIteration++;
                }
                else if (CheckForSpaceDivisionXAxis(spaceToCheck))
                {
                    DivideSpaceXAxis(spaceToCheck);
                    currentIteration++;
                }
                else if (CheckForSpaceDivisionYAxis(spaceToCheck))
                {
                    DivideSpaceYAxis(spaceToCheck);
                    currentIteration++;
                }
                else
                {
                    SpacesToDivide.Remove(spaceToCheck);
                    SpacesToPrint.Add(spaceToCheck);
                }
            }

            SpacesToPrint.AddRange(SpacesToDivide);
        }

        private void DivideSpaceXAxis(SpaceArea spaceToCheck)
        {
            SpacesToDivide.Remove(spaceToCheck);

            int randomXValue = random.Range(
                spaceToCheck.BottomLeftSpaceCorner.x + (_maxRoomWidth + 1),
                spaceToCheck.BottomRightSpaceCorner.x - (_maxRoomWidth + 1)
            );

            if (randomXValue <= spaceToCheck.BottomLeftSpaceCorner.x || randomXValue >= spaceToCheck.TopRightSpaceCorner.x)
            {
                Debug.LogWarning("Invalid X-axis division point. Skipping this space.");
                return;
            }

            SpaceArea newSpaceLeftSide = new SpaceArea(
                spaceToCheck.BottomLeftSpaceCorner,
                new Vector2Int(randomXValue, spaceToCheck.TopRightSpaceCorner.y)
            );

            SpaceArea newSpaceRightSide = new SpaceArea(
                new Vector2Int(randomXValue, spaceToCheck.BottomLeftSpaceCorner.y),
                spaceToCheck.TopRightSpaceCorner
            );

            SpacesToDivide.Add(newSpaceLeftSide);
            SpacesToDivide.Add(newSpaceRightSide);
        }

        private void DivideSpaceYAxis(SpaceArea spaceToCheck)
        {
            SpacesToDivide.Remove(spaceToCheck);

            int randomYValue = random.Range(
                spaceToCheck.BottomLeftSpaceCorner.y + (_maxRoomLength + 1),
                spaceToCheck.TopLeftSpaceCorner.y - (_maxRoomWidth + 1)
            );

            if (randomYValue <= spaceToCheck.BottomLeftSpaceCorner.y || randomYValue >= spaceToCheck.TopRightSpaceCorner.y)
            {
                Debug.LogWarning("Invalid Y-axis division point. Skipping this space.");
                return;
            }

            SpaceArea newSpaceDownSide = new SpaceArea(
                spaceToCheck.BottomLeftSpaceCorner,
                new Vector2Int(spaceToCheck.BottomRightSpaceCorner.x, randomYValue)
            );

            SpaceArea newSpaceUpSide = new SpaceArea(
                newSpaceDownSide.TopLeftSpaceCorner,
                spaceToCheck.TopRightSpaceCorner
            );

            SpacesToDivide.Add(newSpaceUpSide);
            SpacesToDivide.Add(newSpaceDownSide);
        }

        private bool CheckForSpaceDivisionXAxis(SpaceArea spaceToCheck)
        {
            return spaceToCheck.Width > (_maxRoomWidth + 1) * 2;
        }

        private bool CheckForSpaceDivisionYAxis(SpaceArea spaceToCheck)
        {
            return spaceToCheck.Length > (_maxRoomLength + 1) * 2;
        }
    }
}
