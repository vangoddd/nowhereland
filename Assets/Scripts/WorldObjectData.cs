using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldObjectData {
  public int objectID;
  public float[] position = new float[2];
  public int status;

  public WorldObjectData(int id, Vector2 position, int status) {
    this.objectID = id;
    this.position[0] = position.x;
    this.position[1] = position.y;
    this.status = status;
  }
}
