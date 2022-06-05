using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour {
  public PlayerStats _playerStat;
  public ItemInteraction _itemInteraction;
  public Inventory _inventory;
  public MapSO _map;

  int worldObjectLayerMask;

  public Transform crosshair;
  private bool crosshairShown = false;

  private Vector2 locationSnap;

  public AudioRef placeSound;

  void OnEnable() {
    _itemInteraction.OnItemPlaceAction.AddListener(PlaceItem);
    _itemInteraction.OnPlaceableSelected.AddListener(ShowCrosshair);
  }

  void OnDisable() {
    _itemInteraction.OnItemPlaceAction.RemoveListener(PlaceItem);
    _itemInteraction.OnPlaceableSelected.RemoveListener(ShowCrosshair);
  }

  void Awake() {
    worldObjectLayerMask = LayerMask.GetMask("WorldObject", "Water");
  }

  void PlaceItem(Placeable item, int index) {
    if (CanPlaceItem()) {
      Debug.Log("Spawning " + item.worldObjectPrefab.ToString());
      GameObject temp = Instantiate(item.worldObjectPrefab);
      temp.transform.position = locationSnap;
      _inventory.RemoveItemAtSlot(index, 1);
      AudioManager.Instance.PlayOneShot(placeSound);
    }
  }

  bool CanPlaceItem() {
    Collider2D[] hitCollider = Physics2D.OverlapBoxAll((Vector2)locationSnap, new Vector2(.5f, .5f), 0f, worldObjectLayerMask);
    foreach (Collider2D o in hitCollider) {
      Debug.Log(o.gameObject.name);
    }
    if (hitCollider.Length == 0 && _map.tileMap[(int)locationSnap.x][(int)locationSnap.y] == 1) {
      return true;
    } else {
      return false;
    }
  }

  void ShowCrosshair(bool selection) {
    crosshair.gameObject.SetActive(selection);
    crosshairShown = selection;
  }

  void Update() {
    locationSnap.x = Mathf.Round(_playerStat.position.x + _playerStat.playerDirection.x);
    locationSnap.y = Mathf.Round(_playerStat.position.y + _playerStat.playerDirection.y);

    if (crosshairShown) {
      crosshair.position = locationSnap;
    }
  }
}
