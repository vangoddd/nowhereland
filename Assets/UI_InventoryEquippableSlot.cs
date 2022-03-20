using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryEquippableSlot : MonoBehaviour {
  public Image image;
  public Sprite normalSprite, emptySprite;

  public void setEmpty() {
    image.sprite = normalSprite;
  }

  public void setItem() {
    image.sprite = emptySprite;
  }
}
