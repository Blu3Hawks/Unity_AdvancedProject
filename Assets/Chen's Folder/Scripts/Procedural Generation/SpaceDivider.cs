using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;

public class SpaceDivider
{
    private int levelWidth;
    private int levelLength;
    private int maxRoomWidth;
    private int maxRoomLength;
    private int levelIterations;

    public List<SpaceArea> SpacesToDivide { get; private set; } = new List<SpaceArea>();
    public List<SpaceArea> SpacesToPrint { get; private set; } = new List<SpaceArea>();

    public SpaceDivider(int levelWidth, int levelLength, int levelIterations, int maxRoomWidth, int maxRoomLength)
    {
        this.levelWidth = levelWidth;
        this.levelLength = levelLength;
        this.levelIterations = levelIterations;
        this.maxRoomWidth = maxRoomWidth;
        this.maxRoomLength = maxRoomLength;
    }

    public void GenerateAllSpaces()
    {
        SpaceArea firstSpace = new SpaceArea(new Vector2Int(0, 0), new Vector2Int(levelWidth, levelLength));
        SpacesToDivide.Add(firstSpace);
        int currentIteration = 0;

        while (currentIteration < levelIterations && SpacesToDivide.Count > 0)
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
            spaceToCheck.bottomLeftSpaceCorner.x + (maxRoomWidth + 1),
            spaceToCheck.bottomRightSpaceCorner.x - (maxRoomWidth + 1)
        );

        if (randomXValue <= spaceToCheck.bottomLeftSpaceCorner.x || randomXValue >= spaceToCheck.topRightSpaceCorner.x)
        {
            Debug.LogWarning("Invalid X-axis division point. Skipping this space.");
            return;
        }

        SpaceArea newSpaceLeftSide = new SpaceArea(
            spaceToCheck.bottomLeftSpaceCorner,
            new Vector2Int(randomXValue, spaceToCheck.topRightSpaceCorner.y)
        );

        SpaceArea newSpaceRightSide = new SpaceArea(
            new Vector2Int(randomXValue, spaceToCheck.bottomLeftSpaceCorner.y),
            spaceToCheck.topRightSpaceCorner
        );

        SpacesToDivide.Add(newSpaceLeftSide);
        SpacesToDivide.Add(newSpaceRightSide);
    }

    private void DivideSpaceYAxis(SpaceArea spaceToCheck)
    {
        SpacesToDivide.Remove(spaceToCheck);

        int randomYValue = random.Range(
            spaceToCheck.bottomLeftSpaceCorner.y + (maxRoomLength + 1),
            spaceToCheck.topLeftSpaceCorner.y - (maxRoomWidth + 1)
        );

        if (randomYValue <= spaceToCheck.bottomLeftSpaceCorner.y || randomYValue >= spaceToCheck.topRightSpaceCorner.y)
        {
            Debug.LogWarning("Invalid Y-axis division point. Skipping this space.");
            return;
        }

        SpaceArea newSpaceDownSide = new SpaceArea(
            spaceToCheck.bottomLeftSpaceCorner,
            new Vector2Int(spaceToCheck.bottomRightSpaceCorner.x, randomYValue)
        );

        SpaceArea newSpaceUpSide = new SpaceArea(
            newSpaceDownSide.topLeftSpaceCorner,
            spaceToCheck.topRightSpaceCorner
        );

        SpacesToDivide.Add(newSpaceUpSide);
        SpacesToDivide.Add(newSpaceDownSide);
    }

    private bool CheckForSpaceDivisionXAxis(SpaceArea spaceToCheck)
    {
        return spaceToCheck.Width > (maxRoomWidth + 1) * 2;
    }

    private bool CheckForSpaceDivisionYAxis(SpaceArea spaceToCheck)
    {
        return spaceToCheck.Length > (maxRoomLength + 1) * 2;
    }
}
