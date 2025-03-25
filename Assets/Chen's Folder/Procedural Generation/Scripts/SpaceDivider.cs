using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDivider
{
    private int levelWidth;
    private int levelLength;

    private int maxRoomWidth;
    private int maxRoomLength;

    private int levelIterations;
    public List<SpaceArea> spacesToDivide = new List<SpaceArea>();
    public List<SpaceArea> spacesToPrint = new List<SpaceArea>();

    DungeonLevelGenerator levelGenerator;
    public SpaceDivider(int levelWidth, int levelLength, int levelIterations, int maxRoomWidth, int maxRoomLength)
    {
        this.levelWidth = levelWidth;
        this.levelLength = levelLength;
        this.levelIterations = levelIterations;
        this.maxRoomWidth = maxRoomWidth;
        this.maxRoomLength = maxRoomLength;
    }
    /*  public void GenerateAllSpaces()
      {
          // getting the firt space here - then dividing it for the amount of iterations, each time creating new spaces, that are on a list - after dividing remove them from the list. each time
          // check a random space to divide until run out of divisions/ iterations or running out of space to divide.
          Space firstSpace = new Space(new Vector2Int(0, 0), new Vector2Int(levelWidth, levelLength));
          spacesToDivide.Add(firstSpace);
          int currentIteration = 0;

          while (currentIteration < levelIterations && spacesToDivide.Count > 0)
          {
              int randomSpaceToDivide = UnityEngine.Random.Range(0, spacesToDivide.Count);
              Space spaceToCheck = spacesToDivide[randomSpaceToDivide];

              if (CheckForSpaceDivisionXAxis(spaceToCheck) && CheckForSpaceDivisionYAxis(spaceToCheck) && spacesToDivide.Contains(spaceToCheck))
              {
                  //if both options are available then I want it to be randomized.
                  if (UnityEngine.Random.Range(0, 2) == 0)
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
                  spacesToDivide.Remove(spaceToCheck);
                  spacesToPrint.Add(spaceToCheck);
                  continue;
              }
          }


          //now after the while loop is broken we can add the remaining spaces to the list that we print out - our final spaces
          foreach (Space space in spacesToDivide)
          {
              spacesToPrint.Add(space);
          }

          foreach (Space space in spacesToPrint)
          {
              levelGenerator.GenerateRoom(space);
          }
      }


      private void DivideSpaceXAxis(Space spaceToCheck)
      {
          //first we remove it from the list
          spacesToDivide.Remove(spaceToCheck);
          // take the total width - between the min x value + (max room length + 1) and the max x value - (max room length +1)

          int randomXValue = UnityEngine.Random.Range(
              spaceToCheck.bottomLeftSpaceCorner.x + (maxRoomWidth + 1),
              spaceToCheck.bottomRightSpaceCorner.x - (maxRoomWidth + 1)
              );

          if (randomXValue <= spaceToCheck.bottomLeftSpaceCorner.x || randomXValue >= spaceToCheck.topRightSpaceCorner.x)
          {
              Debug.LogWarning("Invalid X-axis division point. Skipping this space.");
              return;
          }
          //now we create two new spaces - the first one, that will be the left part of the divided space, will be having the bottom left and top left corners,
          //combined with two new points - the top right would be the top left + the newXValue and the bottom right will be the bottom left + newXValue

          Space newSpaceLeftSide = new Space(
              spaceToCheck.bottomLeftSpaceCorner,
              new Vector2Int(randomXValue,
              spaceToCheck.topRightSpaceCorner.y)
              );
          //next we will take the other side and make a space out of it - so we will take the middle values, which are the previous space that we have created - we will take its right points
          //as our left points for the new space, and the right points would be the given space right points. Watch the files to get a better understanding for the divisions.

          //we can use the new space on the left - and take its bottom right point, and the given space's top right point to create this new space. That totally works.

          Space newSpaceRightSide = new Space(
              new Vector2Int(randomXValue, spaceToCheck.bottomLeftSpaceCorner.y),
              spaceToCheck.topRightSpaceCorner
              );
          //now we will add these two spaces to the list

          spacesToDivide.Add(newSpaceLeftSide);
          spacesToDivide.Add(newSpaceRightSide);
      }

      private void DivideSpaceYAxis(Space spaceToCheck)
      {
          spacesToDivide.Remove(spaceToCheck);
          //take the total length and randomise between the min y value + max room length + 1 and the max y value - (max room length +1)

          int randomYValue = UnityEngine.Random.Range(
              spaceToCheck.bottomLeftSpaceCorner.y + (maxRoomLength + 1),
              spaceToCheck.topLeftSpaceCorner.y - (maxRoomWidth + 1)
              );

          if (randomYValue <= spaceToCheck.bottomLeftSpaceCorner.y || randomYValue >= spaceToCheck.topRightSpaceCorner.y)
          {
              Debug.LogWarning("Invalid Y-axis division point. Skipping this space.");
              return;
          }

          //now we can create two new spaced - the upper space and bottom. first we make the upper one, so we will make the upper one first
          //it will have the top left and top right corners and then the new bottom left and bottom right points would be
          //the original bottom left and right corners + the new Y Value given.

          Space newSpaceDownSide = new Space(
              spaceToCheck.bottomLeftSpaceCorner,
              new Vector2Int(spaceToCheck.bottomRightSpaceCorner.x, randomYValue)
              );
          //now we do the same with the new space at the bottom

          Space newSpaceUpSide = new Space(
              newSpaceDownSide.topLeftSpaceCorner,
              spaceToCheck.topRightSpaceCorner
              );
          //now we will add these two spaces to the list

          spacesToDivide.Add(newSpaceUpSide);
          spacesToDivide.Add(newSpaceDownSide);
      }


      private bool CheckForSpaceDivisionXAxis(Space spaceToCheck)
      {
          return spaceToCheck.Width > (maxRoomWidth + 1) * 2;
      }

      private bool CheckForSpaceDivisionYAxis(Space spaceToCheck)
      {
          return spaceToCheck.Length > (maxRoomLength + 1) * 2;
      }
    */
}
