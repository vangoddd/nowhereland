using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryEquippableSlot : MonoBehaviour {
  public Image image;
  public Sprite normalSprite, emptySprite;
  public UI_InventoryEquippableContent content;

  public void setEmpty() {
    image.sprite = normalSprite;
    content.setEmpty();
  }

  public void setItem(Item item) {
    image.sprite = emptySprite;
    content.setItem(item);
  }
}
