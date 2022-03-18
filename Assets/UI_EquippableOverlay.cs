using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquippableOverlay : MonoBehaviour {
  public Inventory _inventory;
  public GameObject tools, armor;

  void Start() {
    UpdateVisibility();
  }

  public void UpdateVisibility() {
    if (_inventory.handSlot != null) {
      tools.SetActive(false);
    } else {
      tools.SetActive(true);
    }

    if (_inventory.armorSlot != null) {
      armor.SetActive(false);
    } else {
      armor.SetActive(true);
    }
  }
}
