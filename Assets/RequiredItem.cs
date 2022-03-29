using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequiredItem : MonoBehaviour {
  public Image itemImage;
  public TextMeshProUGUI itemAmount;
  public GameObject overlay;

  public void UpdateInfo(ItemRecipe recipe, int index, int ownedItem) {
    itemImage.sprite = recipe.requiredItems[index].item.sprite;
    itemAmount.text = ownedItem + " / " + recipe.requiredItems[index].amount.ToString();

    overlay.SetActive(ownedItem < recipe.requiredItems[index].amount);
  }
}
