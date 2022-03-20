using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {
  public Item[] itemList;
  private int slot = 18;
  public ItemInteraction _itemInteraction;
  public ItemDatabase _itemDB;

  public Item handSlot;
  public Item armorSlot;

  public GameEvent OnInventoryUpdate;
  public GameEvent OnEquippableUpdated;

  void OnEnable() {
    itemList = new Item[slot];
    handSlot = null;
    armorSlot = null;

    _itemInteraction.OnItemPickup.AddListener(AddItemListener);
    _itemInteraction.OnItemUse.AddListener(UseItem);
    _itemInteraction.OnUnequip.AddListener(UnequipItem);
    _itemInteraction.OnSlotSwap.AddListener(SlotSwap);
  }

  private void OnDisable() {
    _itemInteraction.OnItemPickup.RemoveListener(AddItemListener);
    _itemInteraction.OnItemUse.RemoveListener(UseItem);
    _itemInteraction.OnUnequip.RemoveListener(UnequipItem);
    _itemInteraction.OnSlotSwap.RemoveListener(SlotSwap);
  }

  public void UseItem(int index) {
    if (itemList[index].itemData is Consumable) {
      itemList[index].itemData.UseItem();
      itemList[index].amount -= 1;
      if (itemList[index].amount == 0) itemList[index] = null;
    } else if (itemList[index].itemData is Tools || itemList[index].itemData is Armor) {
      Item lastUsed = equipItem(itemList[index]);
      itemList[index] = lastUsed;
    }
    OnInventoryUpdate.Raise();
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
            if (itemList[i].amount == 0) itemList[i] = null;
          }
          break;
        }
      }
    }
    OnInventoryUpdate.Raise();
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
      if (itemList[i] != null) Debug.Log(itemList[i].itemData.name + "/" + itemList[i].amount + "/" + itemList[i].durability);
    }
  }

  public Item equipItem(Item item) {
    Item temp = null;
    if (item.itemData is Tools) {
      //equip to hand slot
      temp = handSlot;
      handSlot = item;

    } else if (item.itemData is Armor) {
      //equip to armor slot
      temp = armorSlot;
      armorSlot = item;
    }
    OnEquippableUpdated.Raise();
    OnInventoryUpdate.Raise();

    return temp;
  }

  public void UnequipItem(int index) {
    Item unequippedItem;

    int emptySlot = -1;
    for (int i = 0; i < slot; i++) {
      if (itemList[i] == null) {
        emptySlot = i;
        break;
      }
    }

    if (emptySlot == -1) {
      return; //slot is full, cant unequip
    }

    if (index == 0) {
      //unequip hand
      unequippedItem = handSlot;
      handSlot = null;
    } else {
      //uneqip armor
      unequippedItem = armorSlot;
      armorSlot = null;
    }

    itemList[emptySlot] = unequippedItem;

    OnEquippableUpdated.Raise();
    OnInventoryUpdate.Raise();
  }

  public void SlotSwap(int from, int to) {
    //Unequipping tools / armor
    if (from > 17) {
      if (to > 17) return;
      if (itemList[to] == null) {
        itemList[to] = (from - 18 == 0) ? handSlot : armorSlot;
        if (from - 18 == 0) handSlot = null; else armorSlot = null;
      } else if ((itemList[to].itemData is Tools && from == 18) || (itemList[to].itemData is Armor && from == 19)) {
        itemList[to] = equipItem(itemList[to]);
      } else {
        UnequipItem(from - 18);
      }
    }
    //equipping tools / armor
    else if (to > 17) {
      if (itemList[from].itemData is Tools && (to == 18)) {
        itemList[from] = equipItem(itemList[from]);
      } else if (itemList[from].itemData is Armor && (to == 19)) {
        itemList[from] = equipItem(itemList[from]);
      }
    }
    //moving items
    else {
      if (itemList[to] == null) {
        itemList[to] = itemList[from];
        itemList[from] = null;
      } else {
        if (itemList[to].itemData == itemList[from].itemData) {
          //combine stack
          if (itemList[to].isFull()) {
            //swap position
            Item temp = itemList[from];
            itemList[from] = itemList[to];
            itemList[to] = temp;
          } else {
            //if slot enough
            if (itemList[from].amount <= itemList[to].GetFreeSlot()) {
              itemList[to].amount += itemList[from].amount;
              itemList[from] = null;
            }
            //if slot is not enough 
            else {
              int freeSlot = itemList[to].GetFreeSlot();
              itemList[to].amount += freeSlot;
              itemList[from].amount -= freeSlot;
            }
          }
        } else {
          //swap position
          Item temp = itemList[from];
          itemList[from] = itemList[to];
          itemList[to] = temp;
        }
      }
    }

    OnEquippableUpdated.Raise();
    OnInventoryUpdate.Raise();
  }

  [ContextMenu("Print equipped item")]
  public void PrintEquippedItem() {
    if (handSlot != null) Debug.Log(handSlot.itemData.name);
    if (armorSlot != null) Debug.Log(armorSlot.itemData.name);
  }

  [ContextMenu("Populate inv")]
  public void TestingAddItem() {
    itemList[17] = new Item(itemList[0].itemData, 10);
  }

}
