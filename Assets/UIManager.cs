using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
  public GameObject UI_Inventory;
  public UI_Chest UI_Chest;
  public CraftingController UI_crafting;
  public GameObject pauseMenu;
  public GameObject mapMenu;
  public GameObject gameOver;
  public GameObject hurtOverlay;

  public ItemInteraction _itemInteraction;
  public HealthInteraction _healthInteraction;

  public void OpenInventory() {
    UI_Inventory.SetActive(true);
  }

  void OnEnable() {
    _itemInteraction.OnChestOpen.AddListener(OpenChest);
    _healthInteraction.OnPlayerHurt.AddListener(OnPlayerHurt);
  }

  void OnDisable() {
    _itemInteraction.OnChestOpen.RemoveListener(OpenChest);
    _healthInteraction.OnPlayerHurt.RemoveListener(OnPlayerHurt);
  }

  void OpenChest(int chestId) {
    UI_Chest.chestId = chestId;
    UI_Chest.gameObject.SetActive(true);
  }

  public void OpenCraftingWorkbench() {
    UI_crafting.path = "Workbench";
    UI_crafting.SetButtonText("Craft!");
    UI_crafting.gameObject.SetActive(true);
  }

  public void OpenCraftingCookPot() {
    UI_crafting.path = "Cooking";
    UI_crafting.SetButtonText("Cook!");
    UI_crafting.gameObject.SetActive(true);
  }

  public void OpenCraftingAlchemy() {
    UI_crafting.path = "Alchemy";
    UI_crafting.SetButtonText("Brew!");
    UI_crafting.gameObject.SetActive(true);
  }

  public void OpenCraftingStarter() {
    UI_crafting.path = "Starter";
    UI_crafting.SetButtonText("Craft!");
    UI_crafting.gameObject.SetActive(true);
  }

  public void OnPlayerHurt(float amt) {
    hurtOverlay.SetActive(true);
  }

  public void OpenGameOverHUD() {
    gameOver.SetActive(true);
  }

  public void OpenPauseMenu() {
    pauseMenu.SetActive(true);
  }

  public void OpenMap() {
    mapMenu.SetActive(true);
  }
}
