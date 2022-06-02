using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour {
  public RectTransform panelRectTransform;
  public RectTransform backgroundRT;
  public TextMeshProUGUI itemName, type, desc;
  public Image image;

  public Item selectedItem;
  public int slot;
  public ItemInteraction _itemInteraction;

  public GameObject splitButton;

  public float originalPanelSizeY = 34.39f;

  public void UpdateData(Item item) {
    itemName.text = item.itemData.name;
    type.text = item.itemData.name;
    desc.text = item.itemData.description;

    // type.text = item.itemData.GetType().ToString();
    // if (item.itemData.GetType().ToString() == "ItemData") type.text = "Item";
    // if (item.itemData.GetType().ToString() == "Placeable") type.text = "Structure";

    type.text = item.itemData.getItemTypeString();

    image.sprite = item.itemData.sprite;
    if (selectedItem != null && splitButton.activeSelf) {
      setButtonActive(selectedItem.amount > 1);
    }
    Canvas.ForceUpdateCanvases();
  }

  public void ResizePanel() {
    backgroundRT.sizeDelta = new Vector2(backgroundRT.sizeDelta.x, originalPanelSizeY + desc.GetComponent<RectTransform>().sizeDelta.y);
  }

  public void splitItem() {
    Debug.Log("splitting item : " + selectedItem.itemData.name);
    _itemInteraction.SplitItem(slot);
    this.gameObject.SetActive(false);
  }

  public void setButtonActive(bool status) {
    splitButton.SetActive(status);
  }
}
