using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour {
  public PlayerStats _playerStat;
  public ItemInteraction _itemInteraction;
  public Inventory _inventory;

  int worldObjectLayerMask;

  void OnEnable() {
    _itemInteraction.OnItemPlaceAction.AddListener(PlaceItem);
  }

  void OnDisable() {
    _itemInteraction.OnItemPlaceAction.RemoveListener(PlaceItem);
  }

  void Awake() {
    worldObjectLayerMask = LayerMask.GetMask("WorldObject");
  }

  void PlaceItem(Placeable item, int index) {
    if (CanPlaceItem()) {
      Debug.Log("Spawning " + item.worldObjectPrefab.ToString());
      GameObject temp = Instantiate(item.worldObjectPrefab);
      temp.transform.position = _playerStat.position;
      _inventory.RemoveItemAtSlot(index, 1);
    }
  }

  bool CanPlaceItem() {

    Collider2D[] hitCollider = Physics2D.OverlapBoxAll((Vector2)_playerStat.position, new Vector2(1.2f, 1.2f), 0f, worldObjectLayerMask);
    foreach (Collider2D o in hitCollider) {
      Debug.Log(o.gameObject.name);
    }
    if (hitCollider.Length == 0) {
      return true;
    } else {
      return false;
    }
  }
}
