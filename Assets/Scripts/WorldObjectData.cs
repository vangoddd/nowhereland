using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldObjectData {
  public int objectID;
  public float[] position = new float[2];
  public int progress;

  public WorldObjectData(int id, Vector2 position) {
    this.objectID = id;
    this.position[0] = position.x;
    this.position[1] = position.y;
    this.progress = 0;
  }

  public WorldObjectData(GameObject worldObject) {
    this.objectID = 1;
    this.progress = 1;
    this.position[0] = worldObject.transform.position.x;
    this.position[1] = worldObject.transform.position.y;
  }
}
