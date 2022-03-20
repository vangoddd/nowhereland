using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
  public ItemData itemData;
  public int amount;
  public int durability = -1;

  public Item(ItemData itemData, int amount) {
    this.itemData = itemData;
    this.amount = amount;

    if (itemData is Tools) {
      durability = (itemData as Tools).durability;
    }
    if (itemData is Armor) {
      durability = (itemData as Armor).durability;
    }
  }

  public bool isStackable() {
    return itemData.stackCount != 1;
  }

  public int GetFreeSlot() {
    return itemData.stackCount - amount;
  }

  public bool isFull() {
    return amount == itemData.stackCount;
  }
}
