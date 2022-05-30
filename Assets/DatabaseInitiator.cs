using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseInitiator : MonoBehaviour {
  public ItemDatabase itemDB;
  public WorldObjectDB wODB;

  private Object[] item;
  string itemPath = "Item/";

  private Object[] worldObjects;
  string ObjectPath = "Prefabs/WorldObjects/";

  void OnEnable() {
    LoadItem();
    LoadWO();
  }

  //load Item from assset
  void LoadItem() {
    item = Resources.LoadAll(itemPath, typeof(ItemData));

    itemDB.itemList = new List<ItemData>();

    foreach (Object o in item) {
      ItemData temp = o as ItemData;
      itemDB.itemList.Add(temp);
    }

    itemDB.InitiateDict();
  }

  void LoadWO() {
    worldObjects = Resources.LoadAll(ObjectPath, typeof(GameObject));

    wODB.worldObjects = new List<GameObject>();

    int idCounter = 0;

    foreach (Object o in worldObjects) {
      GameObject temp = o as GameObject;
      wODB.worldObjects.Add(temp);
      temp.GetComponent<WorldObject>().objectID = idCounter;
      idCounter++;
    }

    wODB.InitiateDict();
  }
}
