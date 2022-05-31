using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Armor item")]
public class Armor : ItemData {
  public int durability;
  public float def;

  public override string getItemType() {
    return "Armor";
  }

}
