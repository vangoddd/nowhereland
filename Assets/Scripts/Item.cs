using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
  public ItemData itemData;
  public int amount;

  public Item(ItemData itemData, int amount) {
    this.itemData = itemData;
    this.amount = amount;
  }

  public bool isStackable() {
    return itemData.stackCount == 1;
  }

  public int GetFreeSlot() {
    return itemData.stackCount - amount;
  }
}
