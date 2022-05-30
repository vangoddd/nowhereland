using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseInitiator : MonoBehaviour {
  public ItemDatabase itemDB;
  public WorldObjectDB wODB;

  private Object[] item;
  string basePath = "Item/";

  void OnEnable() {
    LoadItem();
  }

  //load Item from assset
  void LoadItem() {
    item = Resources.LoadAll(basePath, typeof(ItemData));

    itemDB.itemList = new List<ItemData>();

    foreach (Object o in item) {
      ItemData temp = o as ItemData;
      itemDB.itemList.Add(temp);
    }

    itemDB.InitiateDict();
  }
}
