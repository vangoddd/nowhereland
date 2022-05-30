using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleAlgorithm {
  int radiusSquared;

  public List<Vector2Int> GetCircleList(int radius, int posX, int posY) {

    List<Vector2Int> pointList = new List<Vector2Int>();
    radiusSquared = radius * radius;

    int x = radius;
    for (int y = 0; y <= x; y++) {
      if (2 * (RadiusError(x, y, radiusSquared) + 2 * y - 1) + (1 - 2 * x) > 0) {
        x--;
      }
      pointList.Add(new Vector2Int(x, y));
    }

    //reflecting with x = y
    int length = pointList.Count;
    for (int i = length - 1; i >= 0; i--) {
      pointList.Add(new Vector2Int(pointList[i].y, pointList[i].x));
    }

    //removes dupes
    pointList = new List<Vector2Int>(new HashSet<Vector2Int>(pointList));

    //create list of x in each y level
    List<int> yToX = new List<int>();

    int yTrack = -1;
    for (int i = 0; i < pointList.Count; i++) {
      if (pointList[i].y != yTrack) {
        yTrack = pointList[i].y;
        yToX.Add(pointList[i].x);
      }
    }

    //main loop to fill the circle
    List<Vector2Int> result = new List<Vector2Int>();

    for (int j = -radius; j <= radius; j++) {
      int yIndex;
      if (j < 0) {
        yIndex = -j;
      } else {
        yIndex = j;
      }

      for (int i = -yToX[yIndex]; i <= yToX[yIndex]; i++) {
        result.Add(new Vector2Int(i + posX, j + posY));
      }
    }

    return result;
  }


  int RadiusError(int x, int y, int radiusSquared) {
    return (x * x) + (y * y) - (radiusSquared);
  }
}