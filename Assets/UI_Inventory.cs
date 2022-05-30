using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour {
  public Transform InventorySlotTemplate;
  private RectTransform[] slots;
  private UI_InventorySlot[] slotScripts;
  public ItemInteraction _itemInteraction;
  public Inventory _inventory;
  private Image[] images;

  public GameEvent OnInventoryClosed;

  public RectTransform toolSlot, armorSlot;
  public UI_InventoryEquippableSlot toolBG, armorBG;

  public UI_Tooltip TooltipPanel;

  void Awake() {
    slots = new RectTransform[20];
    slotScripts = new UI_InventorySlot[20];
    images = new Image[20];

    _itemInteraction.OnShowTooltip.AddListener(TooltipListener);

    InitiateInventory();
    UpdateInventory();
  }

  void OnEnable() {
    UpdateInventory();
  }

  void OnDisable() {
    OnInventoryClosed.Raise();
  }

  void OnDestroy() {
    _itemInteraction.OnShowTooltip.RemoveListener(TooltipListener);
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

    slots[18] = toolSlot;
    slots[19] = armorSlot;

    slotScripts[18] = toolSlot.GetComponent<UI_InventorySlot>();
    slotScripts[18].setSlotEmpty();
    slotScripts[18].slotIndex = 18;
    images[18] = slots[18].GetComponent<Image>();

    slotScripts[19] = armorSlot.GetComponent<UI_InventorySlot>();
    slotScripts[19].setSlotEmpty();
    slotScripts[19].slotIndex = 19;
    images[19] = slots[19].GetComponent<Image>();

    slotScripts[18].setText("");
    slotScripts[19].setText("");

    TooltipPanel.transform.SetAsLastSibling();

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
      slotScripts[18].setSlotEmpty();
      toolBG.setEmpty();
    } else {
      images[18].sprite = _inventory.handSlot.itemData.sprite;
      slotScripts[18].isEmpty = false;
      toolBG.setItem();
    }

    if (_inventory.armorSlot == null) {
      slotScripts[19].setSlotEmpty();
      armorBG.setEmpty();
    } else {
      images[19].sprite = _inventory.armorSlot.itemData.sprite;
      slotScripts[19].isEmpty = false;
      armorBG.setItem();
    }
  }

  void TooltipListener(int slot) {
    if (_inventory.itemList[slot] == null) return;
    TooltipPanel.UpdateData(_inventory.itemList[slot]);
    TooltipPanel.panelRectTransform.anchoredPosition = slotScripts[slot].GetComponent<RectTransform>().anchoredPosition;
    TooltipPanel.gameObject.SetActive(true);
  }
}
