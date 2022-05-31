using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Chest Handler")]
public class ChestHandler : ScriptableObject {

  //Mapping chest id to chest content
  public Dictionary<int, ChestContent> chestList;

  public ItemDatabase itemDB;

  public int currentlyOpenChest;
  public ItemInteraction _itemInteraction;

  public Inventory _inventory;

  public GameEvent OnInventoryUpdate;

  void OnEnable() {
    ResetValues();

    _itemInteraction.OnChestToInv.AddListener(ChestToInv);
    _itemInteraction.OnInvToChest.AddListener(InvToChest);
    _itemInteraction.OnChestSwap.AddListener(ChestSwap);
  }

  void OnDisable() {
    _itemInteraction.OnChestToInv.RemoveListener(ChestToInv);
    _itemInteraction.OnInvToChest.RemoveListener(InvToChest);
    _itemInteraction.OnChestSwap.RemoveListener(ChestSwap);
  }

  public void ResetValues() {
    chestList = new Dictionary<int, ChestContent>();
  }

  public void AddNewChestToList(Chest chest) {
    int index = chest.gameObject.GetInstanceID();
    chestList.Add(index, new ChestContent());
    chest.chestId = index;
    chest.status = index;
  }

  public void ApplyLoadedData(MapSaveData data) {
    chestList = new Dictionary<int, ChestContent>();
    for (int i = 0; i < data.chestIds.Count; i++) {
      chestList.Add(data.chestIds[i], CreateChestContent(data.chestContents[i]));
    }
  }

  private ChestContent CreateChestContent(InventoryData data) {
    ChestContent temp = new ChestContent();

    Item[] items = new Item[12];

    for (int x = 0; x < 12; x++) {
      if (data.itemId[x] == -1) {
        items[x] = null;
      } else {
        items[x] = new Item(itemDB.itemList[data.itemId[x]], data.itemAmount[x]);
        items[x].durability = data.itemDurability[x];
      }
    }

    temp.itemList = items;

    return temp;
  }

  public InventoryData GenerateChestData(ChestContent data) {
    InventoryData temp = new InventoryData();

    int[] _itemId = new int[12];
    int[] _itemDurability = new int[12];
    int[] _itemAmount = new int[12];

    for (int i = 0; i < 12; i++) {
      _itemId[i] = -1;
      _itemDurability[i] = -1;
      _itemAmount[i] = -1;

      if (data.itemList[i] != null) {
        _itemId[i] = itemDB.itemLookup[data.itemList[i].itemData];
        _itemDurability[i] = data.itemList[i].durability;
        _itemAmount[i] = data.itemList[i].amount;
      }
    }

    temp.itemId = _itemId;
    temp.itemDurability = _itemDurability;
    temp.itemAmount = _itemAmount;

    return temp;
  }

  public void ChestToInv(int from, int to) {
    Debug.Log("move chest item from " + from + " to inv " + to);
    if (to > 17) return;

    SwapItem(ref chestList[currentlyOpenChest].itemList[from], ref _inventory.itemList[to]);

    OnInventoryUpdate.Raise();
  }

  public void InvToChest(int from, int to) {
    Debug.Log("move inv item from " + from + " to chest " + to);

    if (from > 17) return;
    SwapItem(ref _inventory.itemList[from], ref chestList[currentlyOpenChest].itemList[to]);

    OnInventoryUpdate.Raise();
  }

  public void ChestSwap(int from, int to) {
    Debug.Log("move chest item from " + from + " to chest " + to);
    SwapItem(ref chestList[currentlyOpenChest].itemList[from], ref chestList[currentlyOpenChest].itemList[to]);

    OnInventoryUpdate.Raise();
  }

  private void SwapItem(ref Item from, ref Item to) {
    if (to != null && from.itemData == to.itemData && to.isStackable() && to.GetFreeSlot() > 0) {
      int freeSlot = to.GetFreeSlot();
      if (from.amount > freeSlot) {
        to.amount += freeSlot;
        from.amount -= freeSlot;
      } else {
        to.amount += from.amount;
        from = null;
      }
    } else {
      Item temp = to;
      to = from;
      from = temp;
    }

  }

  [ContextMenu("Print all chest")]
  public void PrintAllChest() {
    foreach (var item in chestList) {
      Debug.Log(item.Key);
    }
  }
}
