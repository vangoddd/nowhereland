using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CraftingInfo : MonoBehaviour {
  public ItemRecipe recipe;

  public TextMeshProUGUI itemName;
  public TextMeshProUGUI itemDescription;
  public TextMeshProUGUI itemType;
  public TextMeshProUGUI numberOwned;

  public RequiredItem[] requiredItems;
  public ResultItem resultItem;

  public void UpdateRecipe(ItemRecipe recipe) {
    this.recipe = recipe;

    itemName.text = recipe.result.item.name;
    itemType.text = recipe.result.item.GetType().ToString();
    if (recipe.result.item.GetType().ToString() == "ItemData") itemType.text = "Item";
    if (recipe.result.item.GetType().ToString() == "Placeable") itemType.text = "Structure";
    itemDescription.text = recipe.result.item.description;

    UpdateRequiredItem(null);

    resultItem.UpdateInfo(recipe);
  }

  public void UpdateRequiredItem(List<int> requiredAmount) {
    for (int i = 0; i < 4; i++) {
      if (i >= recipe.requiredItems.Length) {
        requiredItems[i].gameObject.SetActive(false);
      } else {
        requiredItems[i].gameObject.SetActive(true);
        if (requiredAmount == null) {
          requiredItems[i].UpdateInfo(recipe, i, 0);
        } else {
          requiredItems[i].UpdateInfo(recipe, i, requiredAmount[i]);
        }
      }
    }
  }

}
