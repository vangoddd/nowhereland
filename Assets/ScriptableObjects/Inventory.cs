using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {
  public Item[] itemList;
  private int slot = 18;

  void OnEnable() {
    itemList = new Item[slot];
  }

  public void AddItem(ItemData item, int amt) {
    //if item is not stackable
    if (!item.isStackable()) {
      int addedAmt = amt;
      for (int i = 0; i < slot; i++) {
        if (itemList[i] == null) {
          itemList[i] = new Item(item, 1);
          addedAmt--;
          if (addedAmt == 0) break;
        }
      }
    }
    //If item is stackable, look for occurences, then if theres no occurences, add to null slot
    else {
      int firstNull = -1;
      for (int i = 0; i < slot; i++) {
        if (itemList[i] == null) {
          firstNull = i;
        } else {
          if (itemList[i].itemData == item) {
            if (itemList[i].GetFreeSlot() - amt >= 0) {
              itemList[i].amount += amt;
            } else {
              int leftover = amt - itemList[i].GetFreeSlot();
              itemList[i].amount = itemList[i].itemData.stackCount;
              AddItem(item, leftover);
            }
            return;
          }
        }

      }
      if (firstNull == -1) {
        Debug.Log("inventory full");
        return;
      }
    }
  }

  [ContextMenu("Print inv")]
  public void PrintInventory() {
    Debug.Log("printing inv");
    for (int i = 0; i < slot; i++) {
      if (itemList[i] != null) Debug.Log(itemList[i].itemData.name + itemList[i].amount);
    }
  }
}
