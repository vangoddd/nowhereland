using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldItemData {
  public int itemId;
  public int itemAmount;
  public int itemDurability;

  public float[] position = new float[2];
}
