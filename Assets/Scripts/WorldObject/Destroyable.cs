using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : WorldObject {
  public int health;
  public ToolType requiredTool = null;

  void Start() {
    base.InitializeObject();
    status = health;
  }

  public override void Interact(GameObject player) {
    base.Interact(player);
    if (_inventory.handSlot != null) {
      health -= (int)((Tools)_inventory.handSlot.itemData).damage;
    } else {
      health -= 1;
    }

    if (health <= 0) DestroyWorldObject();
  }

  public override bool ToolRangeCheck() {
    if (requiredTool.name == "hand") return true;
    if (_inventory.handSlot == null) return false;
    Tools tool = _inventory.handSlot.itemData as Tools;
    if (tool == null) return false;
    if (tool.type == requiredTool) {
      return true;
    }
    return false;
  }

}
