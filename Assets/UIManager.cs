using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
  public GameObject UI_Inventory;
  public UI_Chest UI_Chest;
  public CraftingController UI_crafting;

  public ItemInteraction _itemInteraction;

  public void OpenInventory() {
    UI_Inventory.SetActive(true);
  }

  void OnEnable() {
    _itemInteraction.OnChestOpen.AddListener(OpenChest);
  }

  void OnDisable() {
    _itemInteraction.OnChestOpen.RemoveListener(OpenChest);
  }

  void OpenChest(int chestId) {
    UI_Chest.chestId = chestId;
    UI_Chest.gameObject.SetActive(true);
  }

  public void OpenCraftingWorkbench() {
    UI_crafting.path = "Workbench";
    UI_crafting.gameObject.SetActive(true);
  }

  public void OpenCraftingCookPot() {
    UI_crafting.path = "Cooking";
    UI_crafting.gameObject.SetActive(true);
  }
}
