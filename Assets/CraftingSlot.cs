using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour {
  public ItemRecipe recipe;
  public Image itemSprite;
  public CraftingController controller;
  public GameObject selection;
  public GameObject overlay;

  void Start() {
    itemSprite.sprite = recipe.result.item.sprite;
  }

  public void OnClick() {
    controller.setSelection(this);
  }

  public void CanCraft(bool isCraftable) {
    if (isCraftable) {
      overlay.SetActive(false);
    } else {
      overlay.SetActive(true);
    }
  }
}
