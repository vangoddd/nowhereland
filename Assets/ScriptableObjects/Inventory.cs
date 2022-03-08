using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {
  public Item[] itemList;
  private int slot = 18;
  public ItemInteraction _itemInteraction;
  public ItemDatabase _itemDB;

  public GameEvent OnInventoryUpdate;

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
          itemList[itemIndex].amount += itemList[itemIndex].GetFreeSlot();
          AddItem(item, addAmount);
        } else {
          itemList[itemIndex].amount += amt;
        }
      }
    }

    //Debug.Log("picked up item with ID : " + _itemDB.itemLookup[item]);

    OnInventoryUpdate.Raise();
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



  public void RemoveItem(ItemData item, int amt) {
    if (amt == 0) return;
    if (GetItemCount(item) >= amt) {
      for (int i = 0; i < slot; i++) {
        if (itemList[i] != null && itemList[i].itemData == item) {
          if (itemList[i].amount < amt) {
            int remainder = amt - itemList[i].amount;
            itemList[i] = null;
            RemoveItem(item, remainder);
          } else {
            itemList[i].amount -= amt;
          }
          break;
        }
      }
    }
  }

  public int GetItemCount(ItemData item) {
    int totalCount = 0;
    for (int i = 0; i < slot; i++) {
      if (itemList[i] != null && itemList[i].itemData == item) totalCount += itemList[i].amount;
    }
    return totalCount;
  }

  public void CraftItem(ItemRecipe recipe) {
    bool itemEnough = true;
    for (int i = 0; i < recipe.requiredItems.Length; i++) {
      if (GetItemCount(recipe.requiredItems[i].item) < recipe.requiredItems[i].amount) {
        itemEnough = false;
        break;
      }
    }
    if (itemEnough) {
      for (int i = 0; i < recipe.requiredItems.Length; i++) {
        RemoveItem(recipe.requiredItems[i].item, recipe.requiredItems[i].amount);
      }
      AddItem(recipe.result.item, recipe.result.amount);
    }

  }

  [ContextMenu("Print inv")]
  public void PrintInventory() {
    Debug.Log("printing inv");
    for (int i = 0; i < slot; i++) {
      if (itemList[i] != null) Debug.Log(itemList[i].itemData.name + itemList[i].amount);
    }
  }
  public ItemData berry;

  [ContextMenu("Remove Item")]
  public void TestRemoveItem() {
    RemoveItem(berry, 12);
    OnInventoryUpdate.Raise();
  }

  [ContextMenu("Add berry")]
  void AddBerry() {
    AddItem(berry, 10);
  }
}
