using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {
  public SpriteRenderer sr;
  public ItemData itemData;
  public ItemInteraction _itemInteraction;
  public int itemAmount = 1;
  public int durability = -1;

  public Item itemInstance;

  public MapSO _map;

  private bool pickupCooldown = true;

  private new string name;
  private string description;
  void Start() {
    ApplyData();
    _map.worldItemData.Add(gameObject);
    StartCoroutine(RemovePickupCooldownAfterDrop());
  }

  IEnumerator RemovePickupCooldownAfterDrop() {
    yield return new WaitForSeconds(1f);
    pickupCooldown = false;
  }

  public void ApplyData() {
    if (itemData) {
      sr.sprite = itemData.sprite;
      name = itemData.name;
      description = itemData.description;
      gameObject.name = "Item_" + itemData.name;

      if (itemData is Tools) {
        durability = ((Tools)itemData).durability;
      } else {
        durability = -1;
      }
    }

    itemInstance = new Item(itemData, itemAmount);
    itemInstance.durability = durability;
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.tag == "Player" && !pickupCooldown) {
      _itemInteraction.PickupItem(this);
    }
  }

  public void DestroyAfterPickup() {
    Destroy(gameObject);
    _map.worldItemData.Remove(gameObject);
  }
}


