using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour {
  public Transform InventorySlotTemplate;
  private RectTransform[] slots;
  private UI_InventorySlot[] slotScripts;
  public Inventory _inventory;
  private Image[] images;

  public GameEvent OnInventoryClosed;

  public UI_InventoryEquippableSlot toolSlot, armorSlot;

  void Awake() {
    slots = new RectTransform[18];
    slotScripts = new UI_InventorySlot[18];
    images = new Image[18];
    InitiateInventory();
    UpdateInventory();
  }

  void OnEnable() {
    UpdateInventory();
  }

  void OnDisable() {
    OnInventoryClosed.Raise();
  }

  public void CloseInventory() {
    this.gameObject.SetActive(false);
  }

  private void InitiateInventory() {
    float x = InventorySlotTemplate.GetComponent<RectTransform>().anchoredPosition.x;
    float y = InventorySlotTemplate.GetComponent<RectTransform>().anchoredPosition.y;
    float xOffset = 24f;
    float yOffset = 25f;

    int yCounter = 0;

    for (int i = 0; i < 18; i++) {
      if (i % 6 == 0 && i != 0) yCounter++;

      RectTransform itemSlotTransform = Instantiate(InventorySlotTemplate, transform).GetComponent<RectTransform>();
      itemSlotTransform.gameObject.SetActive(true);
      itemSlotTransform.anchoredPosition = new Vector2(x + (xOffset * (i - (6 * yCounter))), y + (yCounter * -yOffset));

      slotScripts[i] = itemSlotTransform.GetComponent<UI_InventorySlot>();

      slotScripts[i].slotIndex = i;
      slotScripts[i].setSlotEmpty();
      slots[i] = itemSlotTransform;
      images[i] = itemSlotTransform.GetComponent<Image>();
    }
  }

  public void UpdateInventory() {
    for (int i = 0; i < 18; i++) {
      if (_inventory.itemList[i] == null) {
        slotScripts[i].setSlotEmpty();
      } else {
        slotScripts[i].isEmpty = false;
        images[i].sprite = _inventory.itemList[i].itemData.sprite;

        if (_inventory.itemList[i].isStackable()) {
          slotScripts[i].setText(_inventory.itemList[i].amount.ToString());
        } else {
          slotScripts[i].setText("");
        }
      }
    }

    if (_inventory.handSlot == null) {
      toolSlot.setEmpty();
    } else {
      toolSlot.setItem(_inventory.handSlot);
    }

    if (_inventory.armorSlot == null) {
      armorSlot.setEmpty();
    } else {
      armorSlot.setItem(_inventory.armorSlot);
    }
  }
}
