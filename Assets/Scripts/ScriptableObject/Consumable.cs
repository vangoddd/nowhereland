using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Consumable item")]
public class Consumable : ItemData {
  public float health, hunger, thirst;
  public PlayerStats _playerStats;

  [ContextMenu("Use Food Item")]
  public override void UseItem() {
    base.UseItem();
    _playerStats.addStat(new PlayerStatData(health, hunger, thirst));
  }
}
