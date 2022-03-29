using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultItem : MonoBehaviour {
  public Image itemImage;
  public TextMeshProUGUI itemAmount;
  public GameObject overlay;

  public void UpdateInfo(ItemRecipe recipe) {
    itemImage.sprite = recipe.result.item.sprite;
    if (recipe.result.amount == 1) {
      itemAmount.text = "";
    } else {
      itemAmount.text = recipe.result.amount.ToString();
    }
  }
}
