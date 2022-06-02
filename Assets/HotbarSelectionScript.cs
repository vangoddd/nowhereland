using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelectionScript : MonoBehaviour {
  [SerializeField] private RectTransform rectTransform;

  public ItemInteraction _itemInteraction;
  public Inventory _inventory;

  private int initialX = -64;
  private int gap = 24;

  public int selection = 0;
  private bool isNull = true;

  void OnEnable() {
    _itemInteraction.OnItemSlotClicked.AddListener(ChangeSelection);
    _itemInteraction.OnItemPickup.AddListener(UpdateSelection);
    _itemInteraction.OnSlotSwap.AddListener(SlotSwap);

    UpdateSelectionNull();
  }

  void OnDisable() {
    _itemInteraction.OnItemSlotClicked.RemoveListener(ChangeSelection);
    _itemInteraction.OnItemPickup.RemoveListener(UpdateSelection);
    _itemInteraction.OnSlotSwap.RemoveListener(SlotSwap);
  }

  void SlotSwap(int a, int b) {
    UpdateSelectionNull();
    UpdateSelection();
  }

  public void ItemMoved() {
    UpdateSelectionNull();
    UpdateSelection();
  }

  void UpdateSelectionNull() {
    isNull = _inventory.itemList[selection] == null;
  }

  public void ChangeSelection(int index) {
    if (index >= 4) return;
    if (selection == index) {
      _itemInteraction.UseItem(index);
      UpdateSelectionNull();
      UpdateSelection();
      return;
    }
    rectTransform.localPosition = new Vector3(initialX + (index * gap), rectTransform.localPosition.y, 0f);
    selection = index;

    _itemInteraction.PlaceableSelected(_inventory.itemList[selection] != null && _inventory.itemList[selection].itemData.GetType().ToString() == "Placeable");
    UpdateSelectionNull();
  }

  public void UpdateSelection(ItemScript item) {
    if (isNull) {
      _itemInteraction.PickupNull();
      _itemInteraction.PlaceableSelected(_inventory.itemList[selection] != null && _inventory.itemList[selection].itemData.GetType().ToString() == "Placeable");
      UpdateSelectionNull();
    }
  }

  public void UpdateSelection() {
    _itemInteraction.PickupNull();
    _itemInteraction.PlaceableSelected(_inventory.itemList[selection] != null && _inventory.itemList[selection].itemData.GetType().ToString() == "Placeable");
    UpdateSelectionNull();
  }

}
