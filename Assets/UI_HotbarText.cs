using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HotbarText : MonoBehaviour {
  public TextMeshProUGUI tmPro;
  public ItemInteraction _itemInteraction;
  public Inventory _inventory;

  void OnEnable() {
    _itemInteraction.OnItemSlotClicked.AddListener(ShowText);
  }

  void OnDisable() {
    _itemInteraction.OnItemSlotClicked.RemoveListener(ShowText);
  }

  public void ShowText(int slot) {
    tmPro.color = Color.white;

    tmPro.text = _inventory.itemList[slot].itemData.name;

    LeanTween.value(gameObject, Color.white, Color.clear, 3f).setEase(LeanTweenType.easeInCubic).setOnUpdate((Color col) => {
      tmPro.color = col;
    });
  }
}
