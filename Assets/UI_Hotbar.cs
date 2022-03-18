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
      itemSlotTransform.GetComponent<UI_ItemSlot>().slotIndex = i;
      itemSlots[i] = itemSlotTransform;
    }
  }

  [ContextMenu("Update Hotbar")]
  public void UpdateHotbar() {
    for (int i = 0; i < 4; i++) {
      if (_inventory.itemList[i] == null) itemSlots[i].gameObject.SetActive(false);
      else {
        UpdateTemplateSlot(i);
      }
    }
    UpdateEquippableSlot();
  }

  void UpdateTemplateSlot(int i) {
    itemSlots[i].GetComponent<Image>().sprite = _inventory.itemList[i].itemData.sprite;
    itemSlots[i].gameObject.SetActive(true);

    TextMeshProUGUI tmPro = itemSlots[i].GetComponentInChildren<TextMeshProUGUI>();
    if (_inventory.itemList[i].isStackable()) {
      tmPro.text = _inventory.itemList[i].amount.ToString();
    } else {
      tmPro.text = "";
    }
  }

  void UpdateEquippableSlot() {
    if (_inventory.handSlot != null) {
      itemSlots[4].GetComponent<Image>().sprite = _inventory.handSlot.itemData.sprite;
      itemSlots[4].gameObject.SetActive(true);
    } else {
      itemSlots[4].gameObject.SetActive(false);
    }
    if (_inventory.armorSlot != null) {
      itemSlots[5].GetComponent<Image>().sprite = _inventory.armorSlot.itemData.sprite;
      itemSlots[5].gameObject.SetActive(true);
    } else {
      itemSlots[5].gameObject.SetActive(false);
    }
    TextMeshProUGUI tmPro = itemSlots[4].GetComponentInChildren<TextMeshProUGUI>();
    tmPro.text = "";
    tmPro = itemSlots[5].GetComponentInChildren<TextMeshProUGUI>();
    tmPro.text = "";
  }
}
