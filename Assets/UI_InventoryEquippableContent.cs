using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryEquippableContent : MonoBehaviour {
  public Image image;
  public Sprite empty;

  public bool isEmpty = true;


  public void setItem(Item item) {
    image.sprite = item.itemData.sprite;
    isEmpty = false;
  }

  public void setEmpty() {
    image.sprite = empty;
    isEmpty = true;
  }
}
