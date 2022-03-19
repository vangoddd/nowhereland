using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destroyable : WorldObject {
  public float health;
  public ToolType requiredTool = null;

  public override void Interact(GameObject player) {
    base.Interact(player);
    if (_inventory.handSlot != null) {
      health -= ((Tools)_inventory.handSlot.itemData).damage;
      if (health <= 0) DestroyWorldObject();
    }
  }

  public override bool ToolRangeCheck() {
    if (_inventory.handSlot == null) return false;
    Tools tool = _inventory.handSlot.itemData as Tools;
    if (tool.type == requiredTool) {
      return true;
    }
    return false;
  }



}
