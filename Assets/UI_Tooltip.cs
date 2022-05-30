using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour {
  public RectTransform panelRectTransform;
  public TextMeshProUGUI itemName, type, desc;
  public Image image;

  public void UpdateData(Item item) {
    itemName.text = item.itemData.name;
    type.text = item.itemData.name;
    desc.text = item.itemData.description;

    image.sprite = item.itemData.sprite;
  }
}
