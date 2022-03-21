using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Useable {

  public int chestId = -1;
  public ChestHandler chestHandler;
  public ItemDatabase itemDB;

  void Start() {
    base.InitializeObject();

    chestId = status;

    if (chestId == -1) {
      //add to list
      chestHandler.AddNewChestToList(this);
    } else {
      //load from list
    }
  }

  public override void UseObject() {
    Debug.Log("Using object " + gameObject.name);
  }

  public override void DestroyWorldObject() {
    base.DestroyWorldObject();

    Debug.Log("Dropping chest content : ");
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
      if (content[x] != null) Debug.Log(content[x].itemData.name);
    }
  }

}
