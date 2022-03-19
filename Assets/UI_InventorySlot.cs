using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour {
  public int slotIndex;
  public ItemInteraction _itemInteraction;
  public Image image;
  public Sprite emptySprite;
  public TextMeshProUGUI tmPro;

  public bool isEmpty = true;

  public void OnSlotClick() {
    Debug.Log("clicked slot " + slotIndex);
  }

  public void setText(string text) {
    tmPro.text = text;
  }

  public void setSlotEmpty() {
    image.sprite = emptySprite;
    tmPro.text = "";
    isEmpty = true;
  }
}
