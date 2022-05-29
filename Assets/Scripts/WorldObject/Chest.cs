using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Useable {

  public int chestId = -1;
  public ChestHandler chestHandler;
  public ItemDatabase itemDB;

  public ItemInteraction _itemInteraction;

  void Start() {
    base.InitializeObject();
    chestId = status;
    if (chestId == -1) {
      chestHandler.AddNewChestToList(this);
    }
  }

  public override void UseObject() {
    base.UseObject();
    _itemInteraction.OnChestOpen.Invoke(chestId);
  }

  public override void DestroyWorldObject() {
    base.DestroyWorldObject();

    Debug.Log("Dropping chest content : ");
    for (int i = 0; i < 12; i++) {
      if (chestHandler.chestList[chestId].itemList[i] != null) {
        ItemSpawner.Instance.SpawnItemWithOffset(transform.position, chestHandler.chestList[chestId].itemList[i]);
      }
    }
  }

  [ContextMenu("Add random item")]
  public void DebugPopulateChest() {
    int index = Random.Range(0, 12);
    chestHandler.chestList[chestId].itemList[index] = new Item(itemDB.itemList[Random.Range(0, itemDB.itemList.Count)], 1);
  }

  [ContextMenu("Print content")]
  public void DebugPrintChestContent() {
    Item[] content = chestHandler.chestList[chestId].itemList;
    for (int x = 0; x < 12; x++) {
      if (content[x] != null) Debug.Log(content[x].itemData.name + " Index : " + x);
    }
  }

}
