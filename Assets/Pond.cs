using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : Harvestable {

  public PlayerStats _playerstat;

  public override void Interact(GameObject player) {
    _playerstat.addStat(0f, 0f, 15f);
  }

  public override void OnDataLoad() {
  }
}
