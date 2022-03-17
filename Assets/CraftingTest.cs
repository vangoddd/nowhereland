using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTest : MonoBehaviour {
  public Inventory _inventory;
  public ItemRecipe craftingRecipe;

  [ContextMenu("Craft item")]
  void CraftItem() {
    _inventory.CraftItem(craftingRecipe);
  }
}
