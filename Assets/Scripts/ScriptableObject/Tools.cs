using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Tools item")]
public class Tools : ItemData {
  public ToolType type;
  public int durability;
  public float damage;

  public override string getItemType() {
    return "Tools";
  }
}
