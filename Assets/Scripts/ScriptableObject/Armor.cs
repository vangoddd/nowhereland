using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Armor item")]
public class Armor : ItemData {
  public int durability;
  public float def;

  [ContextMenu("Use Armor Item")]
  public override void UseItem() {
    base.UseItem();

    //equip the armor
  }
}
