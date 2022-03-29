using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour {

  private Object[] recipes;
  private string basePath = "Crafting recipes/";
  public string path;

  public CraftingSlot selection = null;
  public Transform content, template;

  public UI_CraftingInfo craftingInfo;
  public Inventory _inventory;

  private List<CraftingSlot> slots = new List<CraftingSlot>();

  public Button craftButton;

  void OnEnable() {
    GenerateCraftingList();
  }

  void Start() {
    GenerateCraftingList();
  }

  public void GenerateCraftingList() {
    recipes = Resources.LoadAll(basePath + path, typeof(ItemRecipe));

    ResetList();

    foreach (Object o in recipes) {
      ItemRecipe temp = o as ItemRecipe;
      Debug.Log(temp.result.item.name);

      CraftingSlot slot = Instantiate(template, content).GetComponent<CraftingSlot>();
      slot.recipe = temp;
      slot.gameObject.name = temp.name;
      slot.gameObject.SetActive(true);
      slots.Add(slot);

      if (selection == null) {
        setSelection(slot);
      }
    }

    UpdateInfo();
  }

  public void setSelection(CraftingSlot slot) {
    if (selection != null) {
      selection.selection.SetActive(false);
    }

    slot.selection.SetActive(true);
    selection = slot;

    UpdateInfo();
  }

  void UpdateToolTip() {
    craftingInfo.UpdateRecipe(selection.recipe);
    craftingInfo.UpdateRequiredItem(_inventory.GetRequiredItemAmountHeld(selection.recipe));
  }

  public void UpdateCanCraft() {
    foreach (CraftingSlot slot in slots) {
      slot.CanCraft(_inventory.CanCraftItem(slot.recipe));
    }
  }

  public void CraftItem() {
    _inventory.CraftItem(selection.recipe);
    UpdateInfo();
  }

  public void UpdateButton() {
    craftButton.interactable = _inventory.CanCraftItem(selection.recipe);
  }

  public void UpdateInfo() {
    UpdateToolTip();
    UpdateCanCraft();
    UpdateButton();
  }

  public void ResetList() {
    for (int i = slots.Count - 1; i >= 0; i--) {
      Destroy(slots[i].gameObject);
    }
    slots = new List<CraftingSlot>();
    selection = null;
  }

}
