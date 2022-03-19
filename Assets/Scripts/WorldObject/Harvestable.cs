using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Harvestable : WorldObject {
  public int regenRate;

  public override bool ToolRangeCheck() {
    return true;
  }

  public override void Interact(GameObject player) {
    base.Interact(player);
  }
}
