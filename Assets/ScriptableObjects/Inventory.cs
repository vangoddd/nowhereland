using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Inventory")]
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
    ResetValues();

    _itemInteraction.OnItemPickup.AddListener(AddItemListener);
    _itemInteraction.OnItemUse.AddListener(UseItem);
    _itemInteraction.OnUnequip.AddListener(UnequipItem);
    _itemInteraction.OnSlotSwap.AddListener(SlotSwap);
    _itemInteraction.OnItemDrop.AddListener(DropItem);
    _itemInteraction.OnItemDelete.AddListener(DeleteItem);
  }

  private void OnDisable() {
    _itemInteraction.OnItemPickup.RemoveListener(AddItemListener);
    _itemInteraction.OnItemUse.RemoveListener(UseItem);
    _itemInteraction.OnUnequip.RemoveListener(UnequipItem);
    _itemInteraction.OnSlotSwap.RemoveListener(SlotSwap);
    _itemInteraction.OnItemDrop.RemoveListener(DropItem);
    _itemInteraction.OnItemDelete.RemoveListener(DeleteItem);
  }

  public void ResetValues() {
    itemList = new Item[slot];
    handSlot = null;
    armorSlot = null;
  }

  public void UseItem(int index) {
    if (itemList[index].itemData is Consumable) {
      itemList[index].itemData.UseItem();
      itemList[index].amount -= 1;
      if (itemList[index].amount == 0) itemList[index] = null;
    } else if (itemList[index].itemData is Tools || itemList[index].itemData is Armor || (itemList[index].itemData is Weapon)) {
      Item lastUsed = equipItem(itemList[index]);
      itemList[index] = lastUsed;
    } else if (itemList[index].itemData is Placeable) {
      _itemInteraction.OnItemPlaceAction.Invoke(itemList[index].itemData as Placeable, index);
    }
    OnInventoryUpdate.Raise();
  }

  public void AddItemListener(ItemScript item) {
    if (canPickUp(item.itemInstance)) {
      item.DestroyAfterPickup();
      AddItem(item.itemData, item.itemAmount);
    }
  }

  public void AddItem(ItemData item, int amt) {
    if (amt <= 0) return;

    if (amt > item.stackCount) {
      AddItem(item, item.stackCount);
      AddItem(item, amt - item.stackCount);
      return;
    }

    if (!canPickUp(new Item(item, amt))) {
      ItemSpawner.Instance.SpawnItemOnPlayer(new Item(item, amt));
      return;
    }

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

  public bool canPickUp(Item item) {
    for (int i = 0; i < 18; i++) {
      if (itemList[i] == null) return true;
    }
    if (ContainNonFullItem(item.itemData) != -1) return true;

    return false;
  }

  public void RemoveItemAtSlot(int index, int amt) {
    if (itemList[index] != null) {
      itemList[index].amount -= amt;
      if (itemList[index].amount <= 0) {
        itemList[index] = null;
      }
    }
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

  public bool CanCraftItem(ItemRecipe recipe) {
    bool itemEnough = true;
    for (int i = 0; i < recipe.requiredItems.Length; i++) {
      if (GetItemCount(recipe.requiredItems[i].item) < recipe.requiredItems[i].amount) {
        itemEnough = false;
        break;
      }
    }
    return itemEnough;
  }

  public void CraftItem(ItemRecipe recipe) {
    if (CanCraftItem(recipe)) {
      for (int i = 0; i < recipe.requiredItems.Length; i++) {
        RemoveItem(recipe.requiredItems[i].item, recipe.requiredItems[i].amount);
      }
      AddItem(recipe.result.item, recipe.result.amount);
    }
  }

  int GetFirstEmptySlot() {
    for (int i = 0; i < 18; i++) {
      if (itemList[i] == null) return i;
    }
    return -1;
  }

  public void SplitAmt(int slot, int amt) {
    if (itemList[slot].amount <= 1) return;
    int targetSlot = GetFirstEmptySlot();
    if (targetSlot != -1 && itemList[slot].amount > amt) {
      itemList[slot].amount -= amt;
      itemList[targetSlot] = new Item(itemList[slot].itemData, amt);
    }
  }

  public void SplitHalf(int slot) {
    SplitAmt(slot, itemList[slot].amount / 2);
  }

  public void SplitOne(int slot) {
    SplitAmt(slot, 1);
  }

  public void DropItem(int slot) {
    if (slot < 18) {
      ItemSpawner.Instance.SpawnItemOnPlayer(itemList[slot]);
      RemoveItemAtSlot(slot, itemList[slot].amount);
      OnInventoryUpdate.Raise();
    }
  }

  public void DeleteItem(int slot) {
    if (slot < 18) {
      RemoveItemAtSlot(slot, itemList[slot].amount);
      OnInventoryUpdate.Raise();
    }
  }

  public List<int> GetRequiredItemAmountHeld(ItemRecipe recipe) {
    List<int> temp = new List<int>();

    for (int i = 0; i < recipe.requiredItems.Length; i++) {

      int tempAmount = 0;
      for (int j = 0; j < 18; j++) {
        if (itemList[j] == null) continue;
        if (itemList[j].itemData == recipe.requiredItems[i].item) {
          tempAmount += itemList[j].amount;
        }
      }

      temp.Add(tempAmount);
    }
    return temp;
  }

  public int GetTotalHeldItem(ItemData item) {
    int temp = 0;
    for (int i = 0; i < 18; i++) {
      if (itemList[i] == null) continue;
      if (itemList[i].itemData == item) {
        temp += itemList[i].amount;
      }
    }
    return temp;
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
    if (item.itemData is Tools || item.itemData is Weapon) {
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

  public void ApplyLoadedData(InventoryData data) {
    for (int i = 0; i < 18; i++) {
      if (data.itemId[i] != -1) {
        itemList[i] = new Item(_itemDB.itemList[data.itemId[i]], data.itemAmount[i]);
        itemList[i].durability = data.itemDurability[i];
      }
    }

    if (data.itemId[18] != -1) {
      handSlot = new Item(_itemDB.itemList[data.itemId[18]], data.itemAmount[18]);
      handSlot.durability = data.itemDurability[18];
    }
    if (data.itemId[19] != -1) {
      armorSlot = new Item(_itemDB.itemList[data.itemId[19]], data.itemAmount[19]);
      armorSlot.durability = data.itemDurability[19];
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

  [ContextMenu("Fill inv non stack")]
  public void TestFillInventory() {
    AddItem(itemList[0].itemData, 17);
  }

  [ContextMenu("Fill inv stack")]
  public void TestFillInventoryStack() {
    AddItem(itemList[0].itemData, 17 * itemList[0].itemData.stackCount);
  }

  [ContextMenu("test split half")]
  public void testSplitHalf() {
    SplitHalf(0);
  }

  [ContextMenu("test split one")]
  public void testSplitOne() {
    SplitOne(0);
  }

}
