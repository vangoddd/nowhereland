using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Hotbar : MonoBehaviour {
  public Inventory _inventory;

  public Transform ItemSlotTemplate;
  private RectTransform[] itemSlots;

  void Start() {
    itemSlots = new RectTransform[6];
    InitiateHotbar();
    UpdateHotbar();
  }


  private void InitiateHotbar() {
    int x = -60;
    int y = 0;
    float offset = 24f;

    for (int i = 0; i < 6; i++) {
      RectTransform itemSlotTransform = Instantiate(ItemSlotTemplate, transform).GetComponent<RectTransform>();
      itemSlotTransform.gameObject.SetActive(true);
      itemSlotTransform.anchoredPosition = new Vector2(x + offset * i, y);
      itemSlots[i] = itemSlotTransform;
    }
  }

  [ContextMenu("Update Hotbar")]
  public void UpdateHotbar() {
    for (int i = 0; i < 6; i++) {
      if (_inventory.itemList[i] == null) itemSlots[i].gameObject.SetActive(false);
      else {
        itemSlots[i].GetComponent<Image>().sprite = _inventory.itemList[i].itemData.sprite;
        itemSlots[i].gameObject.SetActive(true);

        TextMeshProUGUI tmPro = itemSlots[i].GetComponentInChildren<TextMeshProUGUI>();
        if (_inventory.itemList[i].isStackable()) {
          tmPro.text = _inventory.itemList[i].amount.ToString();
        } else {
          tmPro.text = "";
        }
      }
    }
  }
}
