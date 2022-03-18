using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
  public GameObject UI_Inventory;

  public void OpenInventory() {
    UI_Inventory.SetActive(true);
  }
}
