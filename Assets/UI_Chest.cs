using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Chest : MonoBehaviour {
  public int chestId;
  public ChestHandler chestHandler;

  public Transform ChestSlotTemplate;

  private RectTransform[] slots;
  private UI_ChestSlot[] slotScripts;
  private Image[] images;

  public GameEvent OnChestOpen, OnChestClosed;

  void Awake() {
    slots = new RectTransform[12];
    slotScripts = new UI_ChestSlot[12];
    images = new Image[12];
    InitiateChest();
    UpdateChest();
  }

  void OnEnable() {
    OnChestOpen.Raise();
    chestHandler.currentlyOpenChest = chestId;
    UpdateChest();
  }

  void OnDisable() {
    chestHandler.currentlyOpenChest = -1;
    OnChestClosed.Raise();
  }

  public void CloseChest() {
    this.gameObject.SetActive(false);
  }

  private void InitiateChest() {
    float x = ChestSlotTemplate.GetComponent<RectTransform>().anchoredPosition.x;
    float y = ChestSlotTemplate.GetComponent<RectTransform>().anchoredPosition.y;
    float xOffset = 24f;
    float yOffset = 25f;

    int yCounter = 0;

    for (int i = 0; i < 12; i++) {
      if (i % 6 == 0 && i != 0) yCounter++;

      RectTransform itemSlotTransform = Instantiate(ChestSlotTemplate, transform).GetComponent<RectTransform>();
      itemSlotTransform.gameObject.SetActive(true);
      itemSlotTransform.anchoredPosition = new Vector2(x + (xOffset * (i - (6 * yCounter))), y + (yCounter * -yOffset));

      slotScripts[i] = itemSlotTransform.GetComponent<UI_ChestSlot>();

      slotScripts[i].slotIndex = i;
      slotScripts[i].setSlotEmpty();
      slots[i] = itemSlotTransform;
      images[i] = itemSlotTransform.GetComponent<Image>();
    }
  }

  public void UpdateChest() {
    for (int i = 0; i < 12; i++) {
      if (chestHandler.chestList[chestId].itemList[i] == null) {
        slotScripts[i].setSlotEmpty();
      } else {
        slotScripts[i].isEmpty = false;
        images[i].sprite = chestHandler.chestList[chestId].itemList[i].itemData.sprite;

        if (chestHandler.chestList[chestId].itemList[i].isStackable()) {
          slotScripts[i].setText(chestHandler.chestList[chestId].itemList[i].amount.ToString());
        } else {
          slotScripts[i].setText("");
        }
      }
    }
  }

}
