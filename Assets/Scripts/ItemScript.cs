using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemScript : MonoBehaviour {
  public SpriteRenderer sr;
  public ItemData itemData;
  public ItemInteraction _itemInteraction;
  public int itemAmount = 1;

  private new string name;
  private string description;
  void Start() {
    ApplyData();
  }

  public void ApplyData() {
    if (itemData) {
      sr.sprite = itemData.sprite;
      name = itemData.name;
      description = itemData.description;
      gameObject.name = "Item_" + itemData.name;
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.tag == "Player") {
      Destroy(gameObject);
      _itemInteraction.PickupItem(new Item(itemData, itemAmount));
    }
  }
}

[CustomEditor(typeof(ItemScript))]
public class ItemScriptEditor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    ItemScript item = (ItemScript)target;
    item.ApplyData();
  }
}
