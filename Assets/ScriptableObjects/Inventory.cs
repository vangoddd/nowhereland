using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {
  public Item[] itemList;
  private int slot = 18;
  public ItemInteraction _itemInteraction;

  void OnEnable() {
    itemList = new Item[slot];
    _itemInteraction.OnItemPickup.AddListener(AddItemListener);
  }

  private void OnDisable() {
    _itemInteraction.OnItemPickup.RemoveListener(AddItemListener);
  }

  public void AddItemListener(Item item) {
    AddItem(item.itemData, item.amount);
  }

  public void AddItem(ItemData item, int amt) {
    if (amt <= 0) return;
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
      int itemIndex = ContainNonFullItem(item);
      if (itemIndex == -1) { //if no occurences
        for (int i = 0; i < slot; i++) {
          if (itemList[i] == null) {
            itemList[i] = new Item(item, amt);
            break;
          }
        }
      } else { //add item to occurence
        if (itemList[itemIndex].GetFreeSlot() < amt) { //if added item is greater than free stack in a slot
          int addAmount = amt - itemList[itemIndex].GetFreeSlot();
          itemList[itemIndex].amount += addAmount;
          AddItem(item, amt - addAmount);
        } else {
          itemList[itemIndex].amount += amt;
        }
      }
    }
  }

  public int ContainNonFullItem(ItemData item) {
    int containsItem = -1;
    for (int i = 0; i < slot; i++) {
      if (itemList[i] == null) continue;
      if (itemList[i].itemData == item && itemList[i].GetFreeSlot() > 0) {
        containsItem = i;
      }
    }
    return containsItem;
  }

  [ContextMenu("Print inv")]
  public void PrintInventory() {
    Debug.Log("printing inv");
    for (int i = 0; i < slot; i++) {
      if (itemList[i] != null) Debug.Log(itemList[i].itemData.name + itemList[i].amount);
    }
  }
}
