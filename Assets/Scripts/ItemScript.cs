using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {
  public SpriteRenderer sr;
  public ItemData itemData;

  private new string name;
  private string description;
  // Start is called before the first frame update
  void Start() {
    if (itemData) {
      sr.sprite = itemData.sprite;
      name = itemData.name;
      description = itemData.description;
      gameObject.name = "Item_" + itemData.name;
    }

  }

  // Update is called once per frame
  void Update() {

  }

  public ItemInteraction _itemInteraction;

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.tag == "Player") {
      Destroy(gameObject);
      _itemInteraction.PickupItem(new Item(itemData, 1));
      Debug.Log("Player got " + gameObject.name);
    }
  }
}
