using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
  public Inventory inventory;
  public ItemData berry;

  void Start() {

  }

  void Update() {

  }

  [ContextMenu("Add berry")]
  void AddBerry() {
    inventory.AddItem(berry, 10);
  }
}
