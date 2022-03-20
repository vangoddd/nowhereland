using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HotbarText : MonoBehaviour {
  public TextMeshProUGUI tmPro;
  public ItemInteraction _itemInteraction;
  public Inventory _inventory;

  private int tweenId = -1;
  private int lastClicked = 0;

  void OnEnable() {
    _itemInteraction.OnItemSlotClicked.AddListener(ShowText);
  }

  void OnDisable() {
    _itemInteraction.OnItemSlotClicked.RemoveListener(ShowText);
  }

  public void ShowText(int slot) {
    if (slot > 3) return;
    if (slot == lastClicked) return;

    lastClicked = slot;

    if (tweenId != -1) {
      LeanTween.cancel(tweenId);
    }

    tmPro.color = Color.white;

    tmPro.text = _inventory.itemList[slot].itemData.name;

    tweenId = LeanTween.value(gameObject, Color.white, Color.clear, 3f).setEase(LeanTweenType.easeInCubic).setOnUpdate((Color col) => {
      tmPro.color = col;
    }).id;
  }
}
