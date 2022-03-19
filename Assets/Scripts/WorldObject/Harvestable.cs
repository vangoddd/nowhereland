using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Harvestable : WorldObject {
  public int regenRate;

  public override bool ToolRangeCheck() {
    if (!interactable) return false;
    return true;
  }

  public override void Interact(GameObject player) {
    base.Interact(player);
  }
}
