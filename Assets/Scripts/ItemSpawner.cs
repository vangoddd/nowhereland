
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

  private static ItemSpawner _instance;

  public static ItemSpawner Instance {
    get {
      if (_instance is null) {

      }
      return _instance;
    }
  }

  private void Awake() {
    _instance = this;
  }

  public GameObject worldItemPrefab;
  public ItemDatabase itemDB;

  public Transform playerPos;

  public void SpawnItem(Vector3 pos, Item item) {
    GameObject o = Instantiate(worldItemPrefab);
    ItemScript itemScript = o.GetComponent<ItemScript>();

    o.transform.position = pos;

    itemScript.itemData = item.itemData;
    itemScript.itemAmount = item.amount;
    itemScript.durability = item.durability;
  }

  public void SpawnItem(Vector3 pos, ItemData item) {
    GameObject o = Instantiate(worldItemPrefab);
    ItemScript itemScript = o.GetComponent<ItemScript>();

    o.transform.position = pos;

    itemScript.itemData = item;
  }

  public void spawnItemUnstacked(Vector3 pos, Item item) {
    for (int i = 0; i < item.amount; i++) {
      GameObject o = Instantiate(worldItemPrefab);
      ItemScript itemScript = o.GetComponent<ItemScript>();
      o.transform.position = AddRandomOffset(pos, 0.5f);

      itemScript.itemData = item.itemData;
      itemScript.itemAmount = 1;
      itemScript.durability = item.durability;
    }
  }

  public void spawnDrops(Vector3 pos, LootTable drops) {
    for (int i = 0; i < drops.fixedLoot.Length; i++) {
      spawnItemUnstacked(pos, new Item(drops.fixedLoot[i].item, drops.fixedLoot[i].amount));
    }

    for (int i = 0; i < drops.chanceLoot.Length; i++) {
      if (Random.value <= drops.chanceLoot[i].chance) {
        spawnItemUnstacked(pos, new Item(drops.chanceLoot[i].item, drops.chanceLoot[i].amount));
      }
    }
  }

  public void SpawnItemFromLoadedData(List<WorldItemData> data) {
    for (int i = 0; i < data.Count; i++) {
      Vector3 pos = new Vector3(data[i].position[0], data[i].position[1], 0f);
      Item temp = new Item(itemDB.itemList[data[i].itemId], data[i].itemAmount);
      temp.durability = data[i].itemDurability;
      SpawnItem(pos, temp);
    }
  }

  public void SpawnItemOnPlayer(Item item) {
    SpawnItem(playerPos.position, item);
  }

  private Vector3 AddRandomOffset(Vector3 vec, float amt) {
    vec.x += Random.Range(-amt, amt);
    vec.y += Random.Range(-amt, amt);
    return vec;
  }
}
